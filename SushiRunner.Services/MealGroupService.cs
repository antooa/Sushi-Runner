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
    public class MealGroupService : IMealGroupService
    {
        private readonly IRepository<MealGroup, long> _repository;
        private readonly IMapper _mapper;

        private bool _disposed;

        public MealGroupService(IRepository<MealGroup, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void Create(MealGroupDTO mealGroup)
        {
            var group = _mapper.Map<MealGroupDTO, MealGroup>(mealGroup);
            _repository.Create(group);
            _repository.Save();
        }

        public IEnumerable<MealGroupDTO> GetList()
        {
            var groups = _repository.GetList();
            return groups.Select(group => _mapper.Map<MealGroup, MealGroupDTO>(group)).ToList();
        }

        public MealGroupDTO Get(long id)
        {
           
            var group = _repository.Get(id);
            return _mapper.Map<MealGroup, MealGroupDTO>(group);
        }

        public void Update(MealGroupDTO mealGroup)
        {
            var group = _mapper.Map<MealGroupDTO, MealGroup>(mealGroup);
            _repository.Update(group);
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