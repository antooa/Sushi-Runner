﻿using System;
using System.Collections.ObjectModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Models.ViewModels;
using SushiRunner.Services;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Controllers
{
    public class HomeController : Controller
    {
        private ICrudService<Meal, long> _mealService;
        private OrderService _orderService;

        public HomeController(ICrudService<Meal, long> mealService)
        {
            _mealService = mealService;
        }

        public IActionResult Index()
        {
            var meals = _mealService.GetList();
            var mealModels = new Collection<MealModel>();

            foreach (var meal in meals)
            {
                mealModels.Add(new MealModel(meal.Name, meal.Description, meal.ImagePath));
            }

            return View(
                new HomeModel
                {
                    Meals = mealModels
                });
        }
    }
}