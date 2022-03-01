using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class StoreRepository : BaseRepository, IStoreRepository
    {
        public StoreRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {
        }

        public async Task<Response<StoreOrderDto, OrderParams>> GetOrders(OrderParams orderParams, int userId)
        {
            var orders = from order in DataContext.Orders
                join agent in DataContext.StoresAgents on order.StoreId equals agent.StoreId
                where agent.UserId == userId 
                select order;

            if (!string.IsNullOrWhiteSpace(orderParams.StoreName))
            {
                var inner = PredicateBuilder.False<Order>();
                foreach (var store in orderParams.StoreName.Split(","))
                {
                    inner = inner.Or(o => o.Store.Name.Contains(store));
                }
                orders = orders.Where(inner);
            }

            if (string.IsNullOrEmpty(orderParams.Status))
                orders = orders.Where(o => o.Status == Status.Confirmed);
            else if (orderParams.Status != "All")
                orders = orders.Where(o => o.Status == orderParams.Status);

            orders = orderParams.OrderBy switch
            {
                OrderBy.Oldest => orders.OrderBy(o => o.Created),
                _ => orders.OrderByDescending(o => o.Created)
            };

            var ordersDto = orders
                .ProjectTo<StoreOrderDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            return await Response<StoreOrderDto, OrderParams>.CreateAsync(ordersDto, orderParams);
        }

        public async Task<StoreOrderDto> GetOrder(int userId, int orderId)
        {
            await ValidateStoreAgentByOrder(userId, orderId);

            return await DataContext.Orders.Where(o => o.Id == orderId)
                .ProjectTo<StoreOrderDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<StoreOrderDto> StartDispatchingOrder(int userId, int orderId)
        {
            var order = await DataContext.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.TrackEvents)
                .FirstOrDefaultAsync();
            if (order == null) throw new HttpException("Order not found");

            await ValidateStoreAgent(userId, order.StoreId);

            if (order.Status != Status.Confirmed)
                throw new HttpException($"Only {Status.Confirmed} Orders are allowed to start shipping.");

            order.TrackEvents.Add(new TrackEvent
            {
                Date = DateTime.UtcNow,
                LocationId = order.SourceLocationId,
                Status = Status.AwaitingArrival
            });

            order.Status = Status.Dispatched;
            order.Update = DateTime.UtcNow;

            if (!await SaveChanges())
                throw new HttpException("Failed to dispatch order");

            return await DataContext.Orders.Where(o => o.Id == orderId)
                .ProjectTo<StoreOrderDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
