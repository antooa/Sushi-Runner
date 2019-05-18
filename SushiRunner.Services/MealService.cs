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
    public class MealService : IMealService
    {
        private readonly IRepository<Meal, long> _repository;
        private readonly IMapper _mapper;
        private bool _disposed;

        public MealService(IRepository<Meal, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void Create(MealDTO mealDto)
        {
            var meal = _mapper.Map<MealDTO, Meal>(mealDto);
            _repository.Create(meal);
            _repository.Save();
        }

        public IEnumerable<MealDTO> GetList()
        {
            var meals = _repository.GetList();
            return meals.Select(meal => _mapper.Map<Meal, MealDTO>(meal)).ToList();
        }

        public MealDTO Get(long id)
        {
            var meal = _repository.Get(id);
            return _mapper.Map<Meal, MealDTO>(meal);
        }

        public void Update(MealDTO mealDto)
        {
            var meal = _mapper.Map<MealDTO, Meal>(mealDto);
            _repository.Update(meal);
            _repository.Save();
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
            _repository.Save();
        }

        public IEnumerable<MealDTO> GetByGroup(string groupName)
        {
            var meals = _repository.Search(m => m.MealGroup.Name.Equals(groupName));
            
            return meals.Select(meal => _mapper.Map<Meal, MealDTO>(meal)).ToList();
        }

        public IEnumerable<MealDTO> GetByGroupId(long id)
        {
            var meals = _repository.Search(m => m.MealGroup.Id.Equals(id));

            return meals.Select(meal => _mapper.Map<Meal, MealDTO>(meal)).ToList();
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