using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemResolver : IValueResolver<OrderDTO, Order, ICollection<OrderItem>>
    {
        public ICollection<OrderItem> Resolve(OrderDTO source, Order destination, ICollection<OrderItem> destMember, ResolutionContext context)
        {
            var items = new List<OrderItem>();

            foreach (var itemDTO in source.Items)
            {
                var item = new OrderItem()
                {
                    Id = itemDTO.Id,
                    Amount = itemDTO.Amount,
                    Meal = new Meal()
                    {
                        Description = itemDTO.Meal.Description,
                        Name = itemDTO.Meal.Name,
                        Price = itemDTO.Meal.Price,
                        Weight = itemDTO.Meal.Weight
                    }

                };
                
                items.Add(item);
            }

            return items;
        }
    }
}