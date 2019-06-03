using System.ComponentModel.DataAnnotations;

namespace SushiRunner.ViewModels.Account
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}