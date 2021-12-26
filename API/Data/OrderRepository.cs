using API.DTOs;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<CheckoutDto> CheckOut(int userId, List<CheckoutItem> items)
        {
            if (items == null || !items.Any())
                throw new HttpException("No item available!");

            if (items.GroupBy(i => i.StoreItemId).Any(g => g.Count() > 1))
                throw new HttpException("Duplicate StoreItem is not allowed");

            if (items.Any(i => i.ItemQuantity <= 0))
                throw new HttpException("Invalid item quantity.");

            var store = await DataContext.StoreItems
                .Where(si => si.Id == items.First().StoreItemId)
                .Select(si => si.Store)
                .FirstOrDefaultAsync();

            if (store == null)
                throw new HttpException("Unable to find store.");

            var isValidStore = true;

            foreach (var item in items)
            {
                var storeItem = await DataContext.StoreItems
                    .Where(si => si.Id == item.StoreItemId)
                    .Select(si => new
                    {
                        si.Id,
                        si.StoreId,
                        si.Available,
                        si.Product.Name,
                        si.Product.Amount,
                        si.Product.MaxPerOrder
                    })
                    .FirstOrDefaultAsync();

                if (storeItem == null)
                    throw new HttpException($"Item not available with id:{item.StoreItemId}");

                item.AmountPerUnit = storeItem.Amount;
                item.Total = storeItem.Amount * item.ItemQuantity;
                item.Name = storeItem.Name;
                item.ErrorMessage = null;

                if (storeItem.StoreId != store.Id)
                    isValidStore = false;

                if (storeItem.Available <= 0)
                    item.ErrorMessage = $"The item {storeItem.Name} is sold out!";
                else if (storeItem.Available < item.ItemQuantity)
                    item.ErrorMessage = $"Only {storeItem.Available} unit(s) of {storeItem.Name} are available now!";
                else if (item.ItemQuantity > storeItem.MaxPerOrder)
                    item.ErrorMessage = $"Only maximum of {storeItem.MaxPerOrder} unit(s) can be ordered for {storeItem.Name}";
            }

            var checkout = new CheckoutDto
            {
                Items = items,
                Price = items.Sum(i => i.Total),
                IsValid = isValidStore && items.All(i => string.IsNullOrEmpty(i.ErrorMessage))
            };

            if (!isValidStore)
            {
                checkout.ErrorMessage = "Items belong to more than one store!";
                return checkout;
            }

            checkout.StoreId = store.Id;
            checkout.StoreName = store.Name;

            if (!await UserHasAddress(userId))
            {
                checkout.DeliveryCharge = checkout.Price < 500 ? 60 : 0;
                checkout.IsValid = false;
                checkout.ErrorMessage = "Delivery address not available.";
            }
            else
            {
                checkout.DeliveryCharge = checkout.Price < 500
                    ? await IsInterStateDelivery(userId, store.Id) ? 40 : 60
                    : 0;
            }

            checkout.Total = checkout.Price + checkout.DeliveryCharge;

            return checkout;
        }

        public async Task<int> OrderItems(int userId, List<CheckoutItem> items)
        {
            var order = await CreateOrder(userId, items);

            await using var transaction = await DataContext.Database.BeginTransactionAsync();
            try
            {
                var itemIds = order.OrderItems.Select(i => i.StoreItemId).ToArray();

                var storeItems = await DataContext.StoreItems.Where(i => itemIds.Contains(i.Id)).ToListAsync();

                foreach (var item in order.OrderItems)
                {
                    var storeItem = storeItems.First(i => i.Id == item.StoreItemId);
                    if (storeItem.Available < item.Count)
                        throw new HttpException("Item(s) not available.");
                    storeItem.Available -= item.Count;
                    item.Status = Status.Ordered;
                }

                var from = await DataContext.Accounts.Where(a => a.User.Id == userId).Select(a => a.User.AccountId).SingleAsync();
                var to = await DataContext.Accounts.Where(a => a.Store.Id == order.StoreId).Select(a => a.Store.AccountId).SingleAsync();

                var accTransaction = await ProcessTransaction(from, to, order.TotalAmount, $"Transaction for order {order.Id}");

                order.Status = Status.Ordered;
                order.Transaction = accTransaction;

                await DataContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                order.Status = Status.Failed;
                foreach (var item in order.OrderItems) item.Status = Status.Failed;
                await DataContext.SaveChangesAsync();
                throw new HttpException(ex.Message, ex.InnerException);
            }

            return order.Id;
        }

        private async Task<Order> CreateOrder(int userId, List<CheckoutItem> items)
        {
            var checkout = await CheckOut(userId, items);

            if (!checkout.IsValid)
            {
                throw string.IsNullOrEmpty(checkout.ErrorMessage)
                    ? new HttpException(string.Join("\n", checkout.Items
                        .Where(i => !string.IsNullOrEmpty(i.ErrorMessage))
                        .Select(i => i.ErrorMessage)))
                    : new HttpException(checkout.ErrorMessage);
            }

            var order = new Order
            {
                UserId = userId,
                Type = "Product",
                Created = DateTime.UtcNow,
                Status = Status.Created,
                StoreId = checkout.StoreId,
                TotalAmount = checkout.Total,
                DeliveryCharge = checkout.DeliveryCharge,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in checkout.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    Status = Status.Created,
                    Count = item.ItemQuantity,
                    Price = item.AmountPerUnit,
                    StoreItemId = item.StoreItemId
                });
            }

            await DataContext.Orders.AddAsync(order);

            return await DataContext.SaveChangesAsync() <= 0
                ? throw new HttpException("Failed to create Order!")
                : order;
        }

        public async Task<IEnumerable<UserOrderDto>> GetUserOrders(int userId, BaseParams @params)
        {
            var orders = DataContext.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Created)
                .ProjectTo<UserOrderDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<UserOrderDto>.CreateAsync(orders, @params.PageSize, @params.PageNumber);
        }

        public async Task<UserOrderDto> GetUserOrder(int userId, int orderId)
        {
            return await DataContext.Orders
                .Where(o => o.UserId == userId && o.Id == orderId)
                .ProjectTo<UserOrderDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
