using System;
using System.Collections.Generic;

namespace SushiRunner.Models
{
    public class Order
    {
        public long Id { get; set; }
        public List<OrderItem> Items { get; set; }
        public string CustomerName { get; set; }
        public string PaymentType { get; set; }
        public string Address { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime PlacedAt { get; set; }
        public Courier Courier { get; set; }
        public DateTime DeliveredAt { get; set; }
    }
}