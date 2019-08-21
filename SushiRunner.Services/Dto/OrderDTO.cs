using System.Collections.Generic;
using SushiRunner.Data.Entities;
using System;
using System.Collections;

namespace SushiRunner.Services.Dto
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public User User { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentType { get; set; }
        public string Address { get; set; }
        public DateTime PlacedAt { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime DeliveredAt { get; set; }
        
        public int TotalPrice { get; set; }
    }
}