using System.Collections.Generic;
using SushiRunner.Data.Entities;
using System;

namespace DTO.Models
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string PaymentType { get; set; }
        public string Address { get; set; }
        public DateTime PlacedAt { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Courier Courier { get; set; }
        public DateTime DeliveredAt { get; set; }
    }
}