using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemModelDTOResolver : IValueResolver<OrderModel, OrderDTO, ICollection<OrderItemDTO>>
    {
        public ICollection<OrderItemDTO> Resolve(OrderModel source, OrderDTO destination, ICollection<OrderItemDTO> destMember, ResolutionContext context)
        {
            return source.Items.Select(item => new OrderItemDTO
            {
                Meal = context.Mapper.Map<MealModel, MealDTO>(item.Meal),
                Amount = item.Amount
            }).ToList();
        }
    }
}
