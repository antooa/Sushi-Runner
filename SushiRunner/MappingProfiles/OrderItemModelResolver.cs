using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;

namespace SushiRunner.MappingProfiles
{
    public class OrderItemModelResolver : IValueResolver<OrderDTO, OrderModel, ICollection<OrderItemModel>>
    {
        public ICollection<OrderItemModel> Resolve(OrderDTO source, OrderModel destination, ICollection<OrderItemModel> destMember, ResolutionContext context)
        {
            return source.Items.Select(item => new OrderItemModel()
            {
                Meal = context.Mapper.Map<MealDTO, MealModel>(item.Meal),
                Amount = item.Amount
            }).ToList();
        }
    }
}