using System;
using System.Collections.Generic;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class MealGroupService : IMealGroupService
    {
        private readonly IRepository<MealGroup, long> _repository;

        private bool _disposed;

        public MealGroupService(IRepository<MealGroup, long> repository)
        {
            _repository = repository;
        }

        public void Create(MealGroup mealGroup)
        {
            _repository.Create(mealGroup);
            _repository.Save();
        }

        public IEnumerable<MealGroup> GetList()
        {
            return _repository.GetList();
        }

        public MealGroup Get(long id)
        {
            return _repository.Get(id);
        }

        public void Update(MealGroup mealGroup)
        {
            _repository.Update(mealGroup);
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