using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class OrderController : SushiRunnerBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;


        public OrderController(IAccountService accountService, IOrderService orderService, IMapper mapper)
            : base(accountService)
        {
            _orderService = orderService;
            _mapper = mapper;
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
        [HttpPost]
        public IActionResult Delete(long id)
        {
            _orderService.Delete(id);
            return RedirectToAction("Index", "Moderator");
        }
        
        
    }
}