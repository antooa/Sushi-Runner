using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using SushiRunner.Data;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using Xunit;

namespace SushiRunner.Tests.Services
{
    public class DisabledMealServiceTests
    {
        private readonly Mock<IAppConf> _config;

        [Fact]
        public void CreateTest()
        {
            // Arrange
            var repository = new Mock<IRepository<DisabledMeal, long>>();

            var mapper = new Mock<IMapper>();
            var svc = new DisabledMealService(repository.Object, mapper.Object);
            var expected = new DisabledMealDTO
            {
                Id = 4,
                Description = "desc",
                Name = "testMeal",
                ImagePath = "MealPics/1.jpg",
                Weight = 120,
                Price = 200
            };

            // Act
            svc.Create(expected);

            // Assert
            mapper.Verify(m => m.Map<DisabledMealDTO, DisabledMeal>(It.IsAny<DisabledMealDTO>()), Times.Once());
            repository.Verify(r => r.Create(It.IsAny<DisabledMeal>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }


        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var expected = new DisabledMealDTO
            {
                Id = 4,
                Description = "desc",
                Name = "testMeal",
                ImagePath = "MealPics/1.jpg",
                Weight = 120,
                Price = 200
            };
            var repository = new Mock<IRepository<DisabledMeal, long>>();
            repository.Setup(r => r.Get(expected.Id)).Returns(new DisabledMeal
            {
                Id = 4,
                Description = "desc",
                Name = "testMeal",
                ImagePath = "MealPics/1.jpg",
                Weight = 120,
                Price = 200
            });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<DisabledMeal, DisabledMealDTO>(It.IsAny<DisabledMeal>())).Returns(expected);
            var svc = new DisabledMealService(repository.Object, mapper.Object);

            // Act
            svc.Delete(expected.Id);

            // Assert
            repository.Verify(r => r.Delete(It.IsAny<long>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
    }
}