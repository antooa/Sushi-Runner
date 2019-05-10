using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Models.ViewModels;
using SushiRunner.Services;
using SushiRunner.Services.Interfaces;

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
            return View();
        }

        [HttpGet]
        public IActionResult NewOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.New);
            var orders = new Collection<OrderModel>();
            foreach (var dto in dtos)
            {
                var order = _mapper.Map<OrderDTO, OrderModel>(dto);
                orders.Add(order);
                
            }
            return View(orders);
        }

        [HttpGet]
        public IActionResult InProgressOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.InProgress);
            var orders = new Collection<OrderModel>();
            foreach (var dto in dtos)
            {
                var order = _mapper.Map<OrderDTO, OrderModel>(dto);
                orders.Add(order);
                
            }
            return View(orders);
        }
        
        [HttpGet]
        public IActionResult CompletedOrders()
        {
            var dtos = _orderService.GetByStatus(OrderStatus.Completed);
            var orders = new Collection<OrderModel>();
            foreach (var dto in dtos)
            {
                var order = _mapper.Map<OrderDTO, OrderModel>(dto);
                orders.Add(order);
                
            }
            return View(orders);
        }
        
    }
}