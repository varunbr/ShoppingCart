using API.DTOs;
using API.Entities;
using API.Seed;
using AutoMapper;
using System;
using System.Linq;
using Property = API.Entities.Property;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Entity DTO Mapping

            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName.ToLower()));

            CreateMap<User, UserProfileDto>().ReverseMap();
            CreateMap<User, UserInfoDto>()
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.Photo.Url));

            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.Location.ParentId))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.Location.Parent.ParentId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Location.Parent.Parent.ParentId));
            CreateMap<Location, LocationDto>();
            CreateMap<Location, LocationInfoDto>()
                .ForMember(dest => dest.ParentName, act => act.MapFrom(src => src.Parent.Name))
                .IncludeBase<Location, LocationDto>();

            CreateMap<PayOption, PayOptionDto>();
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.FromAccount.User.UserName))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.ToAccount.User.UserName))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Order.Id));
            CreateMap<BaseParams, TransactionContext>();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProductViews.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<Product, ProductMiniDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProductViews.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<Property, PropertyDto>();
            CreateMap<SearchContext, SearchContextDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.GetPrice(src.PriceFrom, src.PriceTo)));

            CreateMap<ProductView, PhotoDto>();
            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.Category, act => act.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Properties, act => act.Ignore())
                .ForMember(dest => dest.Photos, act => act.MapFrom(src => src.ProductViews));

            CreateMap<Category, CategoryMiniDto>()
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.Photo.Url));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.StoreItem.Product.ProductViews.First(p => p.IsMain).Url))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.StoreItem.Product.Name))
                .ForMember(dest => dest.ProductId, act => act.MapFrom(src => src.StoreItem.ProductId));

            CreateMap<Order, BaseOrderDto>();
            CreateMap<Order, UserOrderDto>()
                .IncludeBase<Order, BaseOrderDto>();
            CreateMap<Order, UserOrderDetailDto>()
                .ForMember(dest => dest.LocationName, act => act.MapFrom(src => src.DestinationLocation.Name))
                .IncludeBase<Order, UserOrderDto>();
            CreateMap<Order, StoreOrderDto>()
                .ForMember(dest => dest.StoreName, act => act.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.SourceLocationName, act => act.MapFrom(src => src.SourceLocation.Name))
                .IncludeBase<Order, BaseOrderDto>();
            CreateMap<Order, TrackOrderDto>()
                .IncludeBase<Order, BaseOrderDto>();
            CreateMap<Order, TrackOrderDetailDto>()
                .ForMember(dest => dest.LocationName, act => act.MapFrom(src => src.DestinationLocation.Name))
                .IncludeBase<Order, BaseOrderDto>();

            CreateMap<TrackEvent, BaseTrackEventDto>()
                .ForMember(dest => dest.LocationName, act => act.MapFrom(src => src.SiteLocation.Name))
                .ForMember(dest => dest.LocationType, act => act.MapFrom(src => src.SiteLocation.Type));
            CreateMap<TrackEvent, TrackEventDto>()
                .ForMember(dest => dest.AgentName, act => act.MapFrom(src => src.Agent.Name))
                .ForMember(dest => dest.AgentUserName, act => act.MapFrom(src => src.Agent.UserName))
                .ForMember(dest => dest.AgentPhotoUrl, act => act.MapFrom(src => src.Agent.Photo.Url))
                .IncludeBase<TrackEvent, BaseTrackEventDto>();

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.StoreItem.Product.Name))
                .ForMember(dest => dest.StoreId, act => act.MapFrom(src => src.StoreItem.StoreId))
                .ForMember(dest => dest.StoreName, act => act.MapFrom(src => src.StoreItem.Store.Name))
                .ForMember(dest => dest.ProductId, act => act.MapFrom(src => src.StoreItem.ProductId))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src.StoreItem.Product.Amount))
                .ForMember(dest => dest.MaxPerOrder, act => act.MapFrom(src => src.StoreItem.Product.MaxPerOrder))
                .ForMember(dest => dest.Available, act => act.MapFrom(src => src.StoreItem.Available))
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.StoreItem.Product.ProductViews.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<StoreAgent, StoreAgentDto>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.User.Photo.Url))
                .ForMember(dest => dest.StoreName, act => act.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.StoreLocation, act => act.MapFrom(src => src.Store.Address.Location.Name));
            CreateMap<TrackAgent, TrackAgentDto>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.User.Photo.Url))
                .ForMember(dest => dest.LocationName, act => act.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.LocationType, act => act.MapFrom(src => src.Location.Type))
                .ForMember(dest => dest.ParentLocationId, act => act.MapFrom(src => src.Location.ParentId))
                .ForMember(dest => dest.ParentLocationName, act => act.MapFrom(src => src.Location.Parent.Name))
                .ForMember(dest => dest.ParentLocationType, act => act.MapFrom(src => src.Location.Parent.Type));

            CreateMap<UserRole, BaseAgentDto>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.User.Photo.Url))
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role.Name));

            CreateMap<Store, StoreInfoDto>()
                .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Address.Location.Name));

            //CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            #endregion

            #region SeedData Entity Mapping

            CreateMap<Country, Location>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => "Country"))
                .ForMember(dest => dest.Children, act => act.MapFrom(src => src.States));
            CreateMap<State, Location>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => "State"))
                .ForMember(dest => dest.Children, act => act.MapFrom(src => src.Cities));
            CreateMap<City, Location>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => "City"))
                .ForMember(dest => dest.Children, act => act.MapFrom(src => src.Areas));
            CreateMap<string, Location>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => "Area"))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src));

            CreateMap<Seed.Property, Property>();
            CreateMap<string, CategoryTag>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => 50))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ToLower()));

            CreateMap<ProductCategory, Photo>()
                .ForMember(dest => dest.Category, act => act.Ignore());
            CreateMap<ProductCategory, Category>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CategoryTags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Properties, act => act.MapFrom(src => src.Properties))
                .ForMember(dest => dest.Children, act => act.MapFrom(src => src.SubCategories))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Category));

            CreateMap<ProductSeed, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Properties, opt => opt.Ignore())
                .ForMember(dest => dest.ProductTags, opt => opt.Ignore());

            #endregion
        }
    }
}
