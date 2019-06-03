using System.ComponentModel.DataAnnotations;

namespace SushiRunner.ViewModels.Account
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long",
            MinimumLength = 5)]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Email address is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long",
            MinimumLength = 5)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long",
            MinimumLength = 5)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter some password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirmation password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
