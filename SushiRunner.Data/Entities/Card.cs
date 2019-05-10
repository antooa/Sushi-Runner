using System.Collections.Generic;

namespace SushiRunner.Data.Entities
{
    public class Card
    {
        public long Id { get; set; }
        public List<CardItem> Items { get; set; } = new List<CardItem>();
        public User User { get; set; }
    }
}
