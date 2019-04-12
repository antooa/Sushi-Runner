using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Models;
using Sushi_Runner.Models.ViewModels;

namespace SushiRunner.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager
                        .PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                    {
                        return RedirectToAction("Index", "Moderator");
                    }
                }
            }

            return View();
        }
    }
}