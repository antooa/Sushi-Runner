using System.Collections.Generic;
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
            var items = new List<CartItemModel>();

            foreach (var item in source.Items)
            {
                var itemModel = new CartItemModel()
                {
                    Amount = item.Amount,
                    Meal = Mapper.Map<MealDTO, MealModel>(item.Meal)
                };
                
                items.Add(itemModel);
            }

            return items;
        }
    }
}