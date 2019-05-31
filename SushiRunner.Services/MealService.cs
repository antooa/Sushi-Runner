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
using SushiRunner.Utilities;

namespace SushiRunner.Services
{
    public class MealService : IMealService
    {
        private readonly IRepository<Meal, long> _repository;
        private readonly IMapper _mapper;
        private bool _disposed;
        private readonly IAppConf _appConf;
        private readonly ICartService _cartService;

        public MealService(IRepository<Meal, long> repository, IMapper mapper, IAppConf appConf,
            ICartService cartService)
        {
            _repository = repository;
            _mapper = mapper;
            _appConf = appConf;
            _cartService = cartService;
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

        public void Create(MealDTO mealDto, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var cropped = ImageTool.CropMaxSquare(Image.FromStream(file.OpenReadStream()));
                var resized = ImageTool.Resize(cropped, 200, 200);
                var uploads = Path.Combine(_appConf.WebRootPath, "pics\\MealPics");
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                mealDto.ImagePath = fileName;
                resized.Save(filePath);
            }

            var book = _mapper.Map<MealDTO, Meal>(mealDto);
            _repository.Create(book);
            _repository.Save();
        }

        public IEnumerable<MealDTO> GetMealsWithCartCheckbox(User user)
        {
            var cart = _cartService.GetByUserOrCreateNew(user);
            return GetList()
                .Select(meal =>
                {
                    meal.IsInCart = cart.Items.Any(c => c.MealId.Equals(meal.Id));
                    return meal;
                });
        }

        public IEnumerable<MealDTO> GetMealsWithCartCheckbox(User user, long mealGroupId)
        {
            var cart = _cartService.GetByUserOrCreateNew(user);
            return GetByGroupId(mealGroupId)
                .Select(meal =>
                {
                    meal.IsInCart = cart.Items.Any(c => c.MealId.Equals(meal.Id));
                    return meal;
                });
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
