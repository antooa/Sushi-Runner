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
        public IActionResult Create([Bind("Name, Description, Weight, Price, MealGroupId")] MealModel meal, IFormFile pic)
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

        [HttpPost]
        public IActionResult AddComment([Bind("Message", "ProductId")] CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
//                var comment = new Comment();
//                comment.Message = commentModel.Message;
                var commentDTO = _mapper.Map<CommentModel, CommentDTO>(commentModel);
                
//                var meal = _mealService.Get(commentModel.ProductId);
//                
//                meal.Comments.Add(commentDTO);
                _mealService.AddComment(commentDTO, commentModel.ProductId);
                
                return RedirectToAction("Details","Meal", new {mealId = commentModel.ProductId});
            }
            return RedirectToAction("Details","Meal");
        }
        
        [Route("/Meal/Details/{mealId}")]
        public IActionResult Details(long mealId)
        {
            if (ModelState.IsValid)
            {
                var mealDTO = _mealService.Get(mealId);
                var meal = _mapper.Map<MealDTO, MealModel>(mealDTO);
                
                MealModelWithComments mealWithComments = new MealModelWithComments();
                mealWithComments.Meal = meal;
                
               
                
                
                return View(meal);    
            }
            return RedirectToAction("Index","Home");
        }
    }
}
