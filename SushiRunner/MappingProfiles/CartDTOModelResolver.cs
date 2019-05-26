using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.MappingProfiles
{
    public class CartDTOModelResolver : IValueResolver<CartDTO, CartModel, IEnumerable<CartItemModel>>
    {
        public IEnumerable<CartItemModel> Resolve(CartDTO source, CartModel destination, IEnumerable<CartItemModel> destMember, ResolutionContext context)
        {
            return source.Items.Select(item => new CartItemModel()
            {
                Amount = item.Amount,
                Meal = context.Mapper.Map<MealDTO, MealModel>(item.Meal)
            }).ToList();
        }
    }
}