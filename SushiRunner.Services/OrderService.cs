using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order, long> _repository;
        private readonly IRepository<OrderItem, long> _orderItemRepository;
        private readonly IMapper _mapper;

        private bool _disposed;

        public OrderService(IRepository<Order, long> repository, IRepository<OrderItem, long> orderItemRepository, IMapper mapper)
        {
            _repository = repository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public void Create(OrderDTO orderDto)
        {
            orderDto.PlacedAt = DateTime.Now;
            orderDto.OrderStatus = OrderStatus.New;
            var order = _mapper.Map<OrderDTO, Order>(orderDto);
            _repository.Create(order);
            _repository.Save();
        }

        public IEnumerable<OrderDTO> GetList()
        {
            var orders = _repository.GetList();
            return orders.Select(order => _mapper.Map<Order, OrderDTO>(order)).ToList();
        }

        public IEnumerable<OrderDTO> GetByStatus(OrderStatus status)
        {
            var orders = _repository.Search(o => o.OrderStatus.Equals(status));

            return orders.Select(order => _mapper.Map<Order, OrderDTO>(order)).ToList();
        }

        /*public void Create(User user, string customerName, string phoneNumber, string paymentType, string address,
            CartDTO cart)
        {
            var orderDto = new OrderDTO
            {
                User = user,
                CustomerName = customerName,
                PhoneNumber = phoneNumber,
                PaymentType = paymentType,
                Address = address,
                Items = cart.Items.Select(item => new OrderItem
                {
                    Meal = item.Meal,
                    Amount = item.Amount
                })
            };
            Create(orderDto);
        }*/

        public OrderDTO Get(long id)
        {
            var order = _repository.Get(id);
            var dto = _mapper.Map<Order, OrderDTO>(order);
            return dto;
        }

        public void UpdateOrderItem(long orderItemId, int amount)
        {
            var orderItem = _orderItemRepository.Get(orderItemId);
            //if (orderItem != null)
            //{
            orderItem.Amount = amount;
                _orderItemRepository.Update(orderItem);
            //}
            _orderItemRepository.Save();
        }

        public void Update(OrderDTO orderDto)
        {
            var order = _mapper.Map<OrderDTO, Order>(orderDto);
            if (order == null)
            {
                Console.WriteLine("ORDER IS NULL");
            }
            
            /*foreach (var orderItem in order.Items)
            {
                _orderItemRepository.Update(orderItem);
            }*/
            Console.WriteLine("ORDER BEFORE REPOSITORY");
            Console.WriteLine(order.Id);
            Console.WriteLine(order.CustomerName);
            Console.WriteLine(order.Address);
            Console.WriteLine(order.PhoneNumber);
            Console.WriteLine("ORDER BEFORE REPOSITORY");
            _repository.Update(order);
            _repository.Save();
        }

        public void Delete(long id)
        {
            var order = Get(id);
            foreach (var item in order.Items)
            {
                var orderItem = _mapper.Map<OrderItemDTO, OrderItem>(item);
                _orderItemRepository.Delete(orderItem.Id);
            }
            _repository.Delete(id);
            _repository.Save();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}