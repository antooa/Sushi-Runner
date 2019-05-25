using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemModelDTOResolver : IValueResolver<OrderModel, OrderDTO, IEnumerable<OrderItemDTO>>
    {
        public IEnumerable<OrderItemDTO> Resolve(OrderModel source, OrderDTO destination, IEnumerable<OrderItemDTO> destMember, ResolutionContext context)
        {
            var items = new List<OrderItemDTO>();
            
            foreach (var item in source.Items)
            {
                var itemModel = new OrderItemDTO() {
                    Meal = Mapper.Map<MealModel, MealDTO>(item.Meal),
                    Amount = item.Amount                 
                };
                items.Add(itemModel);
            }

            return items;
        }
    }
}