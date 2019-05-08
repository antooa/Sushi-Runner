using System;
using System.Collections.Generic;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order, long> _repository;

        private bool _disposed;

        public OrderService(IRepository<Order, long> repository)
        {
            _repository = repository;
        }

        public void Create(Order order)
        {
            _repository.Create(order);
            _repository.Save();
        }

        public IEnumerable<Order> GetList()
        {
            return _repository.GetList();
        }

        public Order Get(long id)
        {
            return _repository.Get(id);
        }

        public void Update(Order order)
        {
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