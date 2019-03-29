using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SushiRunner.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public Meal Meal { get; set; }
        public int Amount { get; set; }

        public int CalculatePrice()
        {
            return Meal.Price * Amount;
        }
    }
}