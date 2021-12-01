using API.DTOs;
using API.Entities;
using API.Seed;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName.ToLower()));

            CreateMap<User, UserProfileDto>().ReverseMap();

            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.Parent.Name))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.Parent.Parent.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Parent.Parent.Parent.Name));

            //Mappings for JSON SeedData to Entity Classes
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

            CreateMap<Seed.Property, Entities.Property>();
            CreateMap<ProductCategory, Category>()
                .ForMember(dest => dest.Properties, act => act.MapFrom(src => src.Properties))
                .ForMember(dest => dest.Children, act => act.MapFrom(src => src.SubCategories))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Category));

            CreateMap<ProductSeed, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Properties, opt => opt.Ignore());
        }
    }
}
