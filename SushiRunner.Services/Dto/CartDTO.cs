using System.Collections.Generic;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Dto
{
    public class CartDTO
    {
        public long Id { get; set; }
        public IEnumerable<CartItemDTO> Items { get; set; }
        public User User { get; set; }
    }
}