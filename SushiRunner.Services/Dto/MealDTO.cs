using System.Collections.Generic;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Dto
{
    public class MealDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public long GroupId { get; set; }
        public MealGroupDTO MealGroup { get; set; }
        
        public long MealGroupId { get; set; }
        public bool IsInCart { get; set; }
        
        public IEnumerable<Comment> Comments { get; set; }
        
        public  bool isShown { get; set; }
    }
}