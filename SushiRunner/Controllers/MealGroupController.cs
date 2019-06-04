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
        
        //TODO: Add more controllers
    }
}