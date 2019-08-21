using System;
using System.Collections.Generic;
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
        private readonly IMealGroupService _mealGroupService;

        public MealController(IMealService service, IMapper mapper, IMealGroupService mealGroupService)
        {
            _mapper = mapper;
            _mealGroupService = mealGroupService;
            _mealService = service;
        }

        public IActionResult Index()
        {
            var dtos = _mealService.GetList();
            var meals = dtos.Select(dto => _mapper.Map<MealDTO, MealModel>(dto)).ToList();
            return View(meals);
        }

        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Moderate()
        {

            var mealDtos = _mealService.GetList();
            var groupDtos = _mealGroupService.GetList();
            var meals = mealDtos.Select(mealDto => _mapper.Map<MealDTO, MealModel>(mealDto));
            var groups = groupDtos.Select(groupDto => _mapper.Map<MealGroupDTO, MealGroupModel>(groupDto));
            var tuple = new Tuple<IEnumerable<MealModel>, IEnumerable<MealGroupModel>>(meals, groups);
            return View(tuple);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Create([Bind("Name, Description, Weight, Price, GroupId")]
            MealModel meal, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newMeal = _mapper.Map<MealModel, MealDTO>(meal);
                    newMeal.MealGroupId = meal.GroupId;
                    _mealService.Create(newMeal, pic);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Moderate", "Meal");
            }

            return View(meal);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Update(MealModel meal, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var changedMeal = _mapper.Map<MealModel, MealDTO>(meal);
                    _mealService.Update(changedMeal, pic);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Moderate");
            }

            return RedirectToAction("Moderate");
            //return View(meal);
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
                    //return View(e.Message);
                }

                return RedirectToAction("Moderate", "Meal");
            }

            return View(meal);
        }
    }
}
