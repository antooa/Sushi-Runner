using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class OrderModel
    {
        public string CustomerName { get; set; }
        public string PaymentType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<OrderItemModel> Items { get; set; }    
    }
}