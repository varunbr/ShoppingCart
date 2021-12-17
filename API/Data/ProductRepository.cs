using API.DTOs;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<ProductDetailDto> GetProduct(int productId, int userId)
        {
            var userLocation = await GetUserLocation(userId);

            var product = await DataContext.Products
                .Where(p => p.Id == productId)
                .ProjectTo<ProductDetailDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (product == null) return null;

            product.DeliveryCharge = product.Amount >= 500 ? 0 : 60;

            if (userLocation != null)
            {
                var stateId = userLocation.Parent.ParentId;
                var storeItem = await DataContext.StoreItems
                    .Where(si => si.ProductId == productId && si.Available > 0)
                    .Where(si => si.Store.Address.Location.Parent.ParentId == stateId)
                    .Select(si => new { si.Store.Name, si.Id })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (storeItem != null)
                {
                    product.DeliveryCharge = product.Amount >= 500 ? 0 : 40;
                    product.StoreItemId = storeItem.Id;
                    product.StoreName = storeItem.Name;
                }
            }

            if (product.Available && string.IsNullOrEmpty(product.StoreName))
            {
                var storeItem = await DataContext.StoreItems
                    .Where(si => si.ProductId == productId && si.Available > 0)
                    .Select(si => new { si.Store.Name, si.Id })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (storeItem != null)
                {
                    product.StoreItemId = storeItem.Id;
                    product.StoreName = storeItem.Name;
                }
            }

            var pv = await DataContext.PropertyValues
                .Where(pv => pv.ProductId == productId)
                .Include(pv => pv.Property)
                .AsNoTracking()
                .ToListAsync();

            product.Properties = pv.GetPropertyValue();

            return product;
        }

        public async Task<CheckoutDto> CheckOut(int userId, List<CheckoutItem> items)
        {
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

            checkout.DeliveryCharge = checkout.Price < 500
                ? await IsInterStateDelivery(userId, store.Id) ? 40 : 60
                : 0;
            checkout.Total = checkout.Price + checkout.DeliveryCharge;

            return checkout;
        }
    }
}
