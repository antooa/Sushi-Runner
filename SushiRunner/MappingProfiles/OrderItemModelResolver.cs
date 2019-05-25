using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemModelResolver : IValueResolver<OrderDTO, OrderModel, ICollection<OrderItemModel>>
    {
        public ICollection<OrderItemModel> Resolve(OrderDTO source, OrderModel destination, ICollection<OrderItemModel> destMember, ResolutionContext context)
        {
            var items = new List<OrderItemModel>();
            
            foreach (var item in source.Items)
            {
                var itemDTO = new OrderItemModel() {
                    Meal = Mapper.Map<MealDTO, MealModel>(item.Meal),
                    Amount = item.Amount};
                items.Add(itemDTO);
            }

            return items;
        }
    }
}