using System.Collections.Generic;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.MappingProfiles
{
    public class CartResolver : IValueResolver<CartDTO, Cart, ICollection<CartItem>>
    {
        public ICollection<CartItem> Resolve(CartDTO source, Cart destination, ICollection<CartItem> destMember, ResolutionContext context)
        {
            var items = new List<CartItem>();

            foreach (var itemDto in source.Items)
            {
                var item = new CartItem()
                {
                    Amount = itemDto.Amount,
                    Meal = Mapper.Map<MealDTO, Meal>(itemDto.Meal)
                };
                
                items.Add(item);
            }

            return items;
        }
    }
}