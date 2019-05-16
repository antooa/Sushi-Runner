using System;
using AutoMapper;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;


namespace SushiRunner.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
       

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public void Create([FromBody] OrderModel orderParams)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<OrderModel, OrderDTO>(orderParams);
                _orderService.Create(dto);
                
            }

            RedirectToAction("Index", "Home");
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