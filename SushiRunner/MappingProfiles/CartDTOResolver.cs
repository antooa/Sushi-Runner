using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class CartDTOResolver : IValueResolver<Cart, CartDTO, IEnumerable<CartItemDTO>>
    {
        public IEnumerable<CartItemDTO> Resolve(Cart source, CartDTO destination, IEnumerable<CartItemDTO> destMember, ResolutionContext context)
        {
            return source?.Items?.Select(item => new CartItemDTO()
            {
                Amount = item.Amount,
                Meal = context.Mapper.Map<Meal, MealDTO>(item.Meal),
                MealId = item.MealId
            }).ToList();
        }
    }
}
