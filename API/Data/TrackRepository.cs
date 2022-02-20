using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class TrackRepository : BaseRepository, ITrackRepository
    {
        public TrackRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {
        }

        public async Task<Response<TrackOrderDto, TrackParams>> GetOrders(int userId, TrackParams trackParams)
        {
            var locations = DataContext.TrackAgents
                .Where(a => a.UserId == userId);

            if (trackParams.Location != null)
            {
                var addedLocation = new List<string>();
                var inner = PredicateBuilder.False<TrackAgent>();
                foreach (var location in trackParams.Location)
                {
                    if (location.TryParseLocation(out var name, out var type))
                    {
                        addedLocation.Add(location);
                        inner = inner.Or(a => a.Location.Name == name && a.Location.Type == type);
                    }
                }
                trackParams.Location = addedLocation;
                locations = locations.Where(inner);
            }

            var locationIds = await locations.Select(a => a.LocationId).ToListAsync();

            var events = DataContext.TrackEvents
                .Where(e => locationIds.Contains(e.LocationId) && !e.Done);

            if (!string.IsNullOrEmpty(trackParams.Status))
                events = events.Where(e => e.Status == trackParams.Status);

            trackParams.TotalCount = await events.CountAsync();

            events = trackParams.OrderBy switch
            {
                OrderBy.Latest => events.OrderByDescending(e => e.Date),
                _ => events.OrderBy(e => e.Date)
            };

            var orderIds = await events.AddPagination(trackParams.PageNumber, trackParams.PageSize)
                .Select(e => e.OrderId)
                .ToListAsync();

            var orders = await DataContext.Orders
                .Where(o => orderIds.Contains(o.Id))
                .ProjectTo<TrackOrderDto>(Mapper.ConfigurationProvider)
                .ToListAsync();

            var result = orders.OrderBy(p => orderIds.IndexOf(p.Id)).ToList();

            return Response<TrackOrderDto, TrackParams>.Create(result, trackParams);
        }

        public async Task<TrackOrderDetailDto> GetOrder(int orderId)
        {
            var order = await DataContext.Orders.Where(o => o.Id == orderId)
                .ProjectTo<TrackOrderDetailDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (order == null) throw new HttpException("Order not found");
            return order;
        }

        public async Task<TrackOrderDetailDto> ReceiveOrder(int userId, int orderId)
        {
            var order = await DataContext.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.TrackEvents)
                .FirstOrDefaultAsync();

            if (order == null) throw new HttpException("Order not found");
            if (order.Status != Status.Dispatched) throw new HttpException($"Order is {order.Status}");

            var trackEvent = order.TrackEvents.FirstOrDefault(e => !e.Done);

            if (trackEvent == null)
                throw new HttpException("Failed to receive order", StatusCodes.Status500InternalServerError);

            await ValidateTrackAgent(userId, trackEvent.LocationId);

            if (trackEvent.Status != Status.AwaitingArrival)
                throw new HttpException($"Order is {trackEvent.Status}");

            trackEvent.Date = DateTime.UtcNow;
            trackEvent.Done = true;
            trackEvent.AgentId = userId;
            trackEvent.Status = Status.Arrived;
            order.Update = DateTime.UtcNow;

            order.TrackEvents.Add(new TrackEvent
            {
                Date = DateTime.UtcNow,
                Status = trackEvent.LocationId == order.DestinationLocationId ? Status.AwaitingDelivery : Status.AwaitingDeparture,
                LocationId = trackEvent.LocationId
            });

            if (!await SaveChanges())
                throw new HttpException("Failed to receive order", StatusCodes.Status500InternalServerError);

            return await GetOrder(orderId);
        }

        public async Task<TrackOrderDetailDto> DispatchOrder(int userId, int orderId)
        {
            var order = await DataContext.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.TrackEvents)
                .FirstOrDefaultAsync();

            if (order == null) throw new HttpException("Order not found");
            if (order.Status != Status.Dispatched) throw new HttpException($"Order is {order.Status}");

            var trackEvent = order.TrackEvents.FirstOrDefault(e => !e.Done);

            if (trackEvent == null)
                throw new HttpException("Failed to dispatch order", StatusCodes.Status500InternalServerError);

            await ValidateTrackAgent(userId, trackEvent.LocationId);

            if (trackEvent.Status == Status.AwaitingDelivery)
                throw new HttpException("Order needs to be delivered from this location");

            if (trackEvent.Status != Status.AwaitingDeparture)
                throw new HttpException($"Order is {trackEvent.Status}");

            trackEvent.Done = true;
            trackEvent.AgentId = userId;
            trackEvent.Date = order.Update = DateTime.UtcNow;
            trackEvent.Status = Status.Departed;

            var location = await GetNextLocation(trackEvent.LocationId, order.DestinationLocationId);
            if (location <= 0)
                throw new HttpException("Failed to get next location", StatusCodes.Status500InternalServerError);

            order.TrackEvents.Add(new TrackEvent
            {
                Date = DateTime.UtcNow,
                Status = Status.AwaitingArrival,
                LocationId = location
            });

            if (!await SaveChanges())
                throw new HttpException("Failed to dispatch order", StatusCodes.Status500InternalServerError);

            return await GetOrder(orderId);
        }

        public async Task<TrackOrderDetailDto> DispatchOrderForDelivery(int userId, int orderId)
        {
            var order = await DataContext.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.TrackEvents)
                .FirstOrDefaultAsync();

            if (order == null) throw new HttpException("Order not found");
            if (order.Status != Status.Dispatched) throw new HttpException($"Order is {order.Status}");

            var trackEvent = order.TrackEvents.FirstOrDefault(e => !e.Done);

            if (trackEvent == null)
                throw new HttpException("Failed to dispatch order", StatusCodes.Status500InternalServerError);

            await ValidateTrackAgent(userId, trackEvent.LocationId);

            if (trackEvent.Status != Status.AwaitingDelivery)
                throw new HttpException($"Order is {trackEvent.Status}");

            trackEvent.Done = true;
            trackEvent.AgentId = userId;
            order.Update = trackEvent.Date = DateTime.UtcNow;
            order.Status = trackEvent.Status = Status.OutForDelivery;
            order.TrackEvents.Add(new TrackEvent
            {
                Date = DateTime.UtcNow,
                Status = Status.AwaitingDelivery,
                LocationId = trackEvent.LocationId,
                AgentId = order.UserId
            });

            if (!await SaveChanges())
                throw new HttpException("Failed to dispatch order for delivery", StatusCodes.Status500InternalServerError);

            return await GetOrder(orderId);
        }
    }
}
