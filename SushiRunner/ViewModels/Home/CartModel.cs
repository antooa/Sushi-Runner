using System.Collections.Generic;

namespace SushiRunner.ViewModels.Home
{
    public class CartModel
    {
        public IEnumerable<CartItemModel> Items { get; set; }
        public MakeOrderFormModel OrderModel { get; set; }
    }
}
