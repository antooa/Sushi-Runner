using System;
using System.Collections.Generic;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class MealService : IMealService
    {
        private readonly IRepository<Meal, long> _repository;

        private bool _disposed;

        public MealService(IRepository<Meal, long> repository)
        {
            _repository = repository;
        }

        public void Create(Meal meal)
        {
            _repository.Create(meal);
            _repository.Save();
        }

        public IEnumerable<Meal> GetList()
        {
            return _repository.GetList();
        }

        public Meal Get(long id)
        {
            return _repository.Get(id);
        }

        public void Update(Meal meal)
        {
            _repository.Update(meal);
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