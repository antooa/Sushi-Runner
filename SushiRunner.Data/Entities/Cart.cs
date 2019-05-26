using System.Collections;
using System.Collections.Generic;

namespace SushiRunner.Data.Entities
{
    public class Cart
    {
        public long Id { get; set; }
        public ICollection<CartItem> Items { get; set; }
        public User User { get; set; }
    }
}
