using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels.Home;
using System;
using Microsoft.AspNetCore.Authorization;
using SushiRunner.Data.Entities;
using SushiRunner.ViewModels;


namespace SushiRunner.Controllers
{
    public class OrderController : SushiRunnerBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMealService _mealService;


        public OrderController(IAccountService accountService, IOrderService orderService, IMapper mapper, 
            IMealService mealService)
            : base(accountService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _mealService = mealService;
        }
        
        private IActionResult HandleRedirect(string redirectPath)
        {
            if (redirectPath == null)
            {
                redirectPath = "/";
            }

            return Redirect(redirectPath);
        }
        
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult OrderItemUpdate(long orderItemId, int amount, string redirectPath)
        {
            //if (ModelState.IsValid)
            //{
            Console.WriteLine("AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT ");
            Console.WriteLine(amount);
            Console.WriteLine("AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT AMOUNT ");

                //var dto = _mapper.Map<OrderItemModel, OrderItemDTO>(orderItem);
                _orderService.UpdateOrderItem(orderItemId, amount);
            //}
            return RedirectToAction(redirectPath);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Update(OrderModel orderParams)
        {
            Console.WriteLine("ORDER MODEL INFO");
            Console.WriteLine(orderParams.CustomerName);
            Console.WriteLine(orderParams.PhoneNumber);
            Console.WriteLine(orderParams.Address);
            Console.WriteLine(orderParams.OrderStatus);
            /*Console.WriteLine(orderParams.Items);
            Console.WriteLine("ORDER ITEM INFO");
            foreach (var item in orderParams.Items)
            {
                
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Meal.Name);
                Console.WriteLine(item.Amount);
                
            }*/
            Console.WriteLine("ORDER ITEM INFO");
            Console.WriteLine("ORDER MODEL INFO");
            //if (ModelState.IsValid)
            //{
                Console.WriteLine("MODEL STATE IS VALID");
                try
                {
                    var dto = _mapper.Map<OrderModel, OrderDTO>(orderParams);
                    _orderService.Update(dto);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            //}

            return RedirectToAction("Index", "Moderator");
        }

        [Authorize(Roles = UserRoles.Moderator)]
        public IActionResult Delete([Bind("Id")] OrderModel orderModel)
        {
            
            //if (ModelState.IsValid)
            //{
                try
                {
                    _orderService.Delete(orderModel.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION EXCEPTION EXCEPTION EXCEPTION EXCEPTION EXCEPTION EXCEPTION ");
                    Console.WriteLine(e.Message);
                    return View("Index", orderModel);
                }

                
            //}
            return RedirectToAction("Index", "Moderator");
        }
        
        
    }
}