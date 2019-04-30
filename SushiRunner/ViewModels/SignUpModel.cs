using System.ComponentModel.DataAnnotations;

namespace SushiRunner.ViewModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long",
            MinimumLength = 5)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter some password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirmation password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}