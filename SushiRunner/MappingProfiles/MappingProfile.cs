using AutoMapper;
using DTO.Models;
using SushiRunner.Data.Entities;
using SushiRunner.Models.ViewModels;

namespace SushiRunner.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderModel, OrderDTO>();
            CreateMap<OrderItemModel, OrderItem>();
            CreateMap<OrderDTO, Order>();
        }
    }
}