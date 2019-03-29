using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Sushi_Runner.Models.ViewModels;

namespace SushiRunner.Controllers
{
    public class HomeController : Controller
    {
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
    }
}