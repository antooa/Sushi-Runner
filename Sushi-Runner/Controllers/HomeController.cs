using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Models;
using Sushi_Runner.Models.ViewModels;

namespace SushiRunner.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var meals = new Collection<MealModel>
            {
                new MealModel("Meal Title",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    "/img/sushi.jpg"),
                new MealModel("Meal Title",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    "/img/sushi.jpg")
            };
            return View(
                new HomeModel
                {
                    Meals = meals
                });
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
                        return Redirect("/Moderator");
                    }
                }
            }

            return View();
        }
    }
}