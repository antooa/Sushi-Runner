using System.ComponentModel.DataAnnotations;

namespace Sushi_Runner.Models.ViewModels
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}