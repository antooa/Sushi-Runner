using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

namespace SushiRunner.Controllers
{
    [Authorize(Roles = UserRoles.Moderator)]
    public class ModeratorController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ModeratorController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction("NewOrders");
        }

        [HttpGet]
        public IActionResult NewOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.New);
            var orders = dtos.Select(dto => _mapper.Map<OrderDTO, OrderModel>(dto)).ToList();             
            return View("Index", orders);
        }

        [HttpGet]
        public IActionResult InProgressOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.InProgress);
            var orders = dtos.Select(dto => _mapper.Map<OrderDTO, OrderModel>(dto)).ToList();   
            return View("Index", orders);
        }
        
        [HttpGet]
        public IActionResult CompletedOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.Completed);
            var orders = dtos.Select(dto => _mapper.Map<OrderDTO, OrderModel>(dto)).ToList();   
            return View("Index", orders);
        }

        [HttpGet]
        public IActionResult CanceledOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.Canceled);
            var orders = dtos.Select(dto => _mapper.Map<OrderDTO, OrderModel>(dto)).ToList();   
            return View("Index", orders);
        }
        
    }
}