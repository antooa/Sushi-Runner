using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class CartResolver : IValueResolver<CartDTO, Cart, IEnumerable<CartItem>>
    {
        public IEnumerable<CartItem> Resolve(CartDTO source, Cart destination, IEnumerable<CartItem> destMember, 
            ResolutionContext context)
        {
            return source.Items.Select(itemDto => new CartItem()
            {
                Amount = itemDto.Amount,
                Meal = context.Mapper.Map<MealDTO, Meal>(itemDto.Meal)
            }).ToList();
        }
    }
}