using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class CartDTOResolver : IValueResolver<Cart, CartDTO, IEnumerable<CartItemDTO>>
    {
        public IEnumerable<CartItemDTO> Resolve(Cart source, CartDTO destination, IEnumerable<CartItemDTO> destMember, ResolutionContext context)
        {
            var items = new List<CartItemDTO>();

            foreach (var item in source.Items)
            {
                var itemDto = new CartItemDTO()
                {
                    Amount = item.Amount,
                    Meal = Mapper.Map<Meal, MealDTO>(item.Meal),
                    MealId = item.MealId
                };
                
                items.Add(itemDto);
            }

            return items;
        }
    }
}