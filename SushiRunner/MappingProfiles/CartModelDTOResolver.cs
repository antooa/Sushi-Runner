using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Services.Dto;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.MappingProfiles
{
    public class CartModelDTOResolver : IValueResolver<CartModel, CartDTO, IEnumerable<CartItemDTO>>
    {
        public IEnumerable<CartItemDTO> Resolve(CartModel source, CartDTO destination, IEnumerable<CartItemDTO> destMember, ResolutionContext context)
        {
            var items = new List<CartItemDTO>();

            foreach (var item in source.Items)
            {
                var itemDTO = new CartItemDTO() {
                    Meal = Mapper.Map<MealModel, MealDTO>(item.Meal),
                    Amount = item.Amount,
                    MealId = item.Meal.Id
                };
                items.Add(itemDTO);
            }

            return items;
        }
    }
}