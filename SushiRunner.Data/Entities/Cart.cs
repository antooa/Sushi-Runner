using System.Collections.Generic;

namespace SushiRunner.Data.Entities
{
    public class Cart
    {
        public long Id { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public User User { get; set; }
    }
}
