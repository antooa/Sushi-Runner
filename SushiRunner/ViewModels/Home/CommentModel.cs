using SushiRunner.Data.Entities;

namespace SushiRunner.ViewModels.Home
{
    public class CommentModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        
        public long ProductId { get; set; }
    }
}