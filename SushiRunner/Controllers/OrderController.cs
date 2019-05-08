using System;
using AutoMapper;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Models.ViewModels;
using SushiRunner.Services;


namespace SushiRunner.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
       

        public OrderController(OrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public void Create(long? id, [FromBody] OrderModel orderParams)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var dto = _mapper.Map<OrderModel, OrderDTO>(orderParams);
                    _orderService.Create(dto);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}