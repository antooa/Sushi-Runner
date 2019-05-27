using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SushiRunner.ViewModels.Home
{
    public class OrderModel
    {
        public long Id { get; set; }
        [Required] public string CustomerName { get; set; }
        [Required] public string PaymentType { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public ICollection<OrderItemModel> Items { get; set; }    
    }
}
