using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemDTOResolver : IValueResolver<Order, OrderDTO, IEnumerable<OrderItemDTO>>
    {
        public IEnumerable<OrderItemDTO> Resolve(Order source, OrderDTO destination, IEnumerable<OrderItemDTO> destMember, ResolutionContext context)
        {
            return source.Items.Select(item => new OrderItemDTO()
            {
                Id = item.Id,
                Meal = context.Mapper.Map<Meal, MealDTO>(item.Meal),
                Amount = item.Amount
            }).ToList();
        }
    }
}