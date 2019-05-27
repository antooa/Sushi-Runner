using System.Collections.Generic;

namespace SushiRunner.ViewModels.Home
{
    public class CartModel
    {
        public IEnumerable<CartItemModel> Items { get; set; }
        public OrderModel OrderModel { get; set; }
    }
}
