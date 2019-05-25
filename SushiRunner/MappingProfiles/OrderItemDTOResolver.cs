using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemDTOResolver : IValueResolver<Order, OrderDTO, IEnumerable<OrderItemDTO>>
    {
        public IEnumerable<OrderItemDTO> Resolve(Order source, OrderDTO destination, IEnumerable<OrderItemDTO> destMember, ResolutionContext context)
        {
            var items = new List<OrderItemDTO>();

            foreach (var item in source.Items)
            {
                var itemDTO = new OrderItemDTO() {
                    Id = item.Id,
                    Meal = Mapper.Map<Meal, MealDTO>(item.Meal),
                    Amount = item.Amount};
                items.Add(itemDTO);
            }

            return items;
        }
    }
}