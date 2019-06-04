using System;
using System.Collections.Generic;
using SushiRunner.Data.Entities;

namespace SushiRunner.ViewModels.Account
{
    public class MyOrdersModel
    {
        public List<OrderItemModel> Orders { get; set; }
    }

    public class OrderItemModel
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime PlacedAt { get; set; }
        public string Address { get; set; }
    }
}
