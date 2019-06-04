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
    public class MealGroupController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IMapper _mapper;
        private readonly IMealGroupService _mealGroupService;
        

        public MealGroupController(IMealService service, IMapper mapper, IMealGroupService mealGroupService)
        {
            _mapper = mapper;
            _mealGroupService = mealGroupService;
            _mealService = service;
        }
        
        // GET
        public IActionResult Index()
        {
            var dtos = _mealGroupService.GetList();
            var groups = dtos.Select(dto => _mapper.Map<MealGroupDTO, MealGroupModel>(dto)).ToList();
            return View(groups);
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

        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Create(MealGroupModel model, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newMeal = _mapper.Map<MealGroupModel, MealGroupDTO>(model);
                    _mealGroupService.Create(newMeal, pic);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Moderate", "MealGroup");
            }

            return View(model);
        }
        
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Update(MealGroupModel model, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newMeal = _mapper.Map<MealGroupModel, MealGroupDTO>(model);
                    _mealGroupService.Create(newMeal, pic);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Moderate", "MealGroup");
            }

            return View(model);
        }
        
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Delete([Bind("Id")] MealGroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _mealGroupService.Delete(groupModel.Id);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Moderate", "MealGroup");
            }

            return View(groupModel);
        }
        
        //TODO: Add more controllers
    }
}