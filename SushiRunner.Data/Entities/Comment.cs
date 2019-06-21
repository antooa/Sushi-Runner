using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SushiRunner.Data.Entities
{
    [Table("Comment")]
    public class Comment
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
    }
}