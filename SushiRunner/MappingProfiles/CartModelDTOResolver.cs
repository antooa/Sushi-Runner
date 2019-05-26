using System.Collections.Generic;
using System.Linq;
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
            return source.Items.Select(item => new CartItemDTO()
            {
                Meal = context.Mapper.Map<MealModel, MealDTO>(item.Meal),
                Amount = item.Amount,
                MealId = item.Meal.Id
            }).ToList();
        }
    }
}