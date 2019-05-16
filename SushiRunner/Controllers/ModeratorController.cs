using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
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
            return RedirectToAction("NewOrders");
        }

        [HttpGet]
        public IActionResult NewOrders()
        {
            var orders = _orderService.GetByStatus(OrderStatus.New);
            return View("Index", orders);
        }

        [HttpGet]
        public IActionResult InProgressOrders()
        {
            var orders = _orderService.GetByStatus(OrderStatus.InProgress);
            return View("Index", orders);
        }
        
        [HttpGet]
        public IActionResult CompletedOrders()
        {
            var orders = _orderService.GetByStatus(OrderStatus.Completed);            
            return View("Index", orders);
        }
        
    }
}