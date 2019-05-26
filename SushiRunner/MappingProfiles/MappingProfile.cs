using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Meal, MealDTO>();
            CreateMap<MealDTO, Meal>();
            CreateMap<MealDTO, MealModel>();
            CreateMap<MealModel, MealDTO>();
            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<OrderItemResolver>());
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<OrderItemDTOResolver>());
            CreateMap<OrderModel, OrderDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<OrderItemModelDTOResolver>());
            CreateMap<OrderDTO, OrderModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<OrderItemModelResolver>());
            CreateMap<CartModel, CartDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<CartModelDTOResolver>());
            CreateMap<CartDTO, CartModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<CartDTOModelResolver>());
            CreateMap<CartDTO, Cart>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<CartResolver>());
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom<CartDTOResolver>());
        }
    }
}