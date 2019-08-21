using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemResolver : IValueResolver<OrderDTO, Order, ICollection<OrderItem>>
    {
        public ICollection<OrderItem> Resolve(OrderDTO source, Order destination, ICollection<OrderItem> destMember, 
            ResolutionContext context)
        {
            return source.Items.Select(itemDTO => new OrderItem
            {
                Id = itemDTO.Id,
                Amount = itemDTO.Amount,
                Meal = context.Mapper.Map<MealDTO, Meal>(itemDTO.Meal)
            }).ToList();
        }
    }
}