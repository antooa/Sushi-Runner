using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SushiRunner.Data.Entities;

namespace SushiRunner.ViewModels.Home
{
    public class OrderModel
    {
        public long Id { get; set; }
        public int TotalPrice { get; set; }
        [Required] public string CustomerName { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string PaymentType { get; set; }
        [Required] public string Address { get; set; }
        public OrderStatus Status { get; set; }
        public string Comment { get; set; }
        public ICollection<OrderItemModel> Items { get; set; }
    }
}
