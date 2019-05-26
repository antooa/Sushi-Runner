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

namespace SushiRunner.Controllers
{
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IMapper _mapper;

        public MealController(IMealService service, IMapper mapper)
        {
            _mapper = mapper;
            _mealService = service;
        }

        public IActionResult Index()
        {
            var dtos = _mealService.GetList();
            var meals = dtos.Select(dto => _mapper.Map<MealDTO, MealModel>(dto)).ToList();
            return View(meals);
        }
        
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Create([Bind("Id, Name, Description, Weight, Price, MealGroupId")] MealModel meal, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newMeal = _mapper.Map<MealModel, MealDTO>(meal);
                    _mealService.Create(newMeal, pic);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(meal);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Update(MealModel meal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var changedMeal = _mapper.Map<MealModel, MealDTO>(meal);
                    _mealService.Update(changedMeal);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Index");
            }

            return View(meal);
        }
        
       
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Delete([Bind("Id")] MealModel meal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _mealService.Delete(meal.Id);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(meal);
        }
    }
}
