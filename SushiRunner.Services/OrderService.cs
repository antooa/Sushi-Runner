using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DTO.Models;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order, long> _repository;
        private readonly IMapper _mapper;
        
        private bool _disposed;

        public OrderService(IRepository<Order, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void Create(OrderDTO orderDto)
        {
            orderDto.PlacedAt = DateTime.Now;
            orderDto.OrderStatus = OrderStatus.WaitingForResponse;
            var order = _mapper.Map<OrderDTO, Order>(orderDto);
            _repository.Create(order);
            _repository.Save();
        }

        public IEnumerable<OrderDTO> GetList()
        {
            var orders = _repository.GetList();
            return orders.Select(order => _mapper.Map<Order, OrderDTO>(order)).ToList();
        }

        public OrderDTO Get(long id)
        {
            var order = _repository.Get(id);
            var dto = _mapper.Map<Order, OrderDTO>(order);
            return dto;
        }

        public void Update(OrderDTO orderDto)
        {
            var order = _mapper.Map<OrderDTO, Order>(orderDto);
            _repository.Update(order);  
            _repository.Save();
        }

        public void Delete(long id)
        {
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