using AutoMapper;
using DTO.Models;
using SushiRunner.Data.Entities;
using SushiRunner.ViewModels;

namespace SushiRunner.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderModel, OrderDTO>();
            CreateMap<OrderItemModel, OrderItem>();
            CreateMap<OrderItem, OrderItemModel>();
        }
    }
}