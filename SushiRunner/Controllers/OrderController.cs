using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;


        public OrderController(IAccountService accountService, IOrderService orderService,
            ICartService cartService, IMapper mapper)
        {
            _accountService = accountService;
            _orderService = orderService;
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderModel orderModel)
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            var cart = _cartService.GetByUserOrCreateNew(user);
            
            var orderDto = new OrderDTO
            {
                CustomerName = orderModel.CustomerName,
                PhoneNumber = orderModel.PhoneNumber,
                PaymentType = orderModel.PaymentType,
                Address = orderModel.Address,
                PlacedAt = DateTime.Now,
                Items = cart.Items.Select(item => new OrderItemDTO()
                {
                    Meal = item.Meal,
                    Amount = item.Amount
                }),
                OrderStatus = OrderStatus.New
            };
            _orderService.Create(orderDto);
            return RedirectToAction("Index", "Home");
        }

        [HttpPut]
        public void Update([FromBody] OrderModel orderParams)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<OrderModel, OrderDTO>(orderParams);
                _orderService.Update(dto);
            }

            RedirectToAction("Index", "Moderator");
        }
    }
}
