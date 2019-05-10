using System;
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
        private IMealService _mealService;

        public HomeController(IMealService mealService)
        {
            _mealService = mealService;
        }

        public IActionResult Index()
        {
            var meals = _mealService.GetList();

            var mealModels = new Collection<MealModel>();
            foreach (var meal in meals)
            {
                mealModels.Add(new MealModel
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Description = meal.Description,
                    Price = meal.Price,
                    ImagePath = meal.ImagePath,
                    Weight = meal.Weight
                });
            }

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

        public IActionResult AddToCart(long id)
        {
            Console.WriteLine(">>>>>>>   <<<<<<<<");
            Console.WriteLine($"Add product with Id={id} to basket");
            Console.WriteLine(">>>>>>>   <<<<<<<<");
            return RedirectToAction("Index");
        }
    }
}
