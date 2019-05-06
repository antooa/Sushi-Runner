using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

namespace SushiRunner.Controllers
{
    public class HomeController : Controller
    {
        private ICrudService<Meal, long> _mealService;

        public HomeController(ICrudService<Meal, long> mealService)
        {
            _mealService = mealService;
        }

        public IActionResult Index()
        {
          //  var meals = _mealService.GetList();
            var mealModels = new Collection<MealModel>();
            mealModels.Add(new MealModel
            {
                Id = 1,
                Name = "Sushi",
                Description = "Tasty shushi",
                ImagePath = "/img/sushi.jpg",
                Price = 100,
                Weight = 120
            });

            //foreach (var meal in meals)
            //{
            //    mealModels.Add(new MealModel(meal.Name, meal.Description, meal.ImagePath));
            //}

            return View(
                new HomeModel
                {
                    Meals = mealModels
                });
        }

        public IActionResult Error()
        {
            return View(
                new ErrorModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }
    }
}