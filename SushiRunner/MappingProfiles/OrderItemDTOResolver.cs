using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemDTOResolver : IValueResolver<Order, OrderDTO, ICollection<OrderItemDTO>>
    {
        public ICollection<OrderItemDTO> Resolve(Order source, OrderDTO destination, ICollection<OrderItemDTO> destMember, ResolutionContext context)
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