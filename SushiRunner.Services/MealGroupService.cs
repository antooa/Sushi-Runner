using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IAppConf _appConf;

        public MealGroupService(IRepository<MealGroup, long> repository, IMapper mapper, IAppConf appConf)
        {
            _repository = repository;
            _mapper = mapper;
            _appConf = appConf;
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

        public void Create(MealGroupDTO groupDto, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var image = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(_appConf.WebRootPath, "img");
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                groupDto.ImagePath = @"/img/" + fileName;
                image.Save(filePath);
            }
            
            
            var group = _mapper.Map<MealGroupDTO, MealGroup>(groupDto);
            _repository.Create(group);
            _repository.Save();
        }

        public void Update(MealGroupDTO groupDto, IFormFile file)
        {
            var oldImageName = Get(groupDto.Id).ImagePath;

            if (file != null && file.Length > 0)
            {
                var image = Image.FromStream(file.OpenReadStream());
                

                var uploads = Path.Combine(_appConf.WebRootPath, "img");
                if (!string.IsNullOrEmpty(oldImageName))
                {
                    var oldPath = Path.Combine(uploads, oldImageName);
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                image.Save(filePath);
                groupDto.ImagePath = @"/img/" + fileName;
            }
            else
            {
                groupDto.ImagePath = oldImageName;
            }
            
            var meal = _mapper.Map<MealGroupDTO, MealGroup>(groupDto);
            _repository.Update(meal);
            _repository.Save();
        }
    }
}
