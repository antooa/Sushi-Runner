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
    public class DisabledMealService : IDisabledMealService
    {
        private readonly IRepository<DisabledMeal, long> _repository;
        private readonly IMapper _mapper;

        private bool _disposed;

        public DisabledMealService(IRepository<DisabledMeal, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public void Create(DisabledMealDTO disabledMealDto)
        {
            var disabledMeal = _mapper.Map<DisabledMealDTO, DisabledMeal>(disabledMealDto);
            _repository.Create(disabledMeal);
            _repository.Save();
        }

        public IEnumerable<DisabledMealDTO> GetList()
        {
            var disabledMeals = _repository.GetList();
            return disabledMeals.Select(meal => _mapper.Map<DisabledMeal, DisabledMealDTO>(meal)).ToList();
        }

        public DisabledMealDTO Get(long id)
        {
            var disabledMeal = _repository.Get(id);
            return _mapper.Map<DisabledMeal, DisabledMealDTO>(disabledMeal);
        }

        public void Update(DisabledMealDTO disabledMealDto)
        {
            var disabledMeal = _mapper.Map<DisabledMealDTO, DisabledMeal>(disabledMealDto);
            _repository.Update(disabledMeal);
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