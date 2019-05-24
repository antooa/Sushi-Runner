using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

namespace SushiRunner.Controllers
{
    public class MealGroupController : Controller
    {
        private readonly IMealGroupService _mealService;
        private readonly IMapper _mapper;
        

        public MealGroupController(IMealGroupService service, IMapper mapper)
        {
            _mapper = mapper;
            _mealService = service;
        }
        
        // GET
        public IActionResult Index()
        {
            var dtos = _mealService.GetList();
            var groups = dtos.Select(dto => _mapper.Map<MealGroupDTO, MealGroupModel>(dto)).ToList();
            return View(groups);
        }
        //TODO: Add more controllers
    }
}