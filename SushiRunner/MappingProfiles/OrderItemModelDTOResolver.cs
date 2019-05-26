using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemModelDTOResolver : IValueResolver<OrderModel, OrderDTO, IEnumerable<OrderItemDTO>>
    {
        public IEnumerable<OrderItemDTO> Resolve(OrderModel source, OrderDTO destination, IEnumerable<OrderItemDTO> destMember, ResolutionContext context)
        {
            return source.Items.Select(item => new OrderItemDTO()
            {
                Meal = context.Mapper.Map<MealModel, MealDTO>(item.Meal),
                Amount = item.Amount
            }).ToList();
        }
    }
}