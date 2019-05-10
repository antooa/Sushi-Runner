using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SushiRunner.Data.Entities
{
    [Table("Order")]
    public class Order
    {
        public long Id { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public string CustomerName { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string PaymentType { get; set; }
        public string Address { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime DeliveredAt { get; set; }
    }
}