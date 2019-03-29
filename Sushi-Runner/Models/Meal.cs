using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SushiRunner.Models
{
    public class Meal
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public MealGroup MealGroup { get; set; }
    }
}