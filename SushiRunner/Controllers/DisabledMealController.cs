using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class DisabledMealController : Controller
    {
        private readonly IDisabledMealService _disabledMealService;
        private readonly IMapper _mapper;

        public DisabledMealController(IDisabledMealService service, IMapper mapper)
        {
            _mapper = mapper;
            _disabledMealService = service;
        }

        public IActionResult Index()
        {
            var dtos = _disabledMealService.GetList();
            var meals = dtos.Select(dto => _mapper.Map<DisabledMealDTO, DisabledMealModel>(dto)).ToList();
            return View(meals);
        }

        [HttpPost]
//        [Authorize(Roles = UserRoles.Customer)]
        public IActionResult Create([Bind("Name, Description, Weight, Price, ImagePath")]
            DisabledMealModel meal)
        {
            if (ModelState.IsValid)
            {
                var newMeal = _mapper.Map<DisabledMealModel, DisabledMealDTO>(meal);
                    _disabledMealService.Create(newMeal);
                

                return RedirectToAction("Index","Home");
            }

           return RedirectToAction("Index","Home");
        }

//        [HttpPost]
//        [Authorize(Roles = UserRoles.Customer)]
//        public IActionResult Update(MealModel meal, IFormFile pic)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var changedMeal = _mapper.Map<MealModel, MealDTO>(meal);
//                    _mealService.Update(changedMeal, pic);
//                }
//                catch (Exception e)
//                {
//                    return View(e.Message);
//                }
//
//                return RedirectToAction("Index");
//            }
//
//            return View(meal);
//        }


//        [Authorize(Roles = UserRoles.Customer)]
        public IActionResult Delete([Bind("Id")] MealModel meal)
        {
            if (ModelState.IsValid)
            {
                
                    _disabledMealService.Delete(meal.Id);
               
                 

               return RedirectToAction(nameof(Index));
            }

           return RedirectToAction(nameof(Index));
        }
    }
}