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
    public class MealServiceTests
    {
        private readonly Mock<IAppConf> _config;

        public MealServiceTests()
        {
            _config = new Mock<IAppConf>();
            _config.Setup(c => c.WebRootPath).Returns(string.Empty);
        }

        [Fact]
        public void GetListTest()
        {
            // Arrange
            var list = GetDtoCollection();
            var svc = SetupService();
            
            // Act
            IEnumerable<MealDTO> actual = svc.GetList();
            
            // Assert
            Assert.Equal(actual.Count(), list.Count());

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetByGroupTest(long id)
        {
            // Arrange
            var list = GetDtoCollection();
            var svc = SetupService();
            var mealDtos = list.ToList();
            var expected = mealDtos.First(dto => dto.MealGroup.Id.Equals(id));
            
            // Act
            var actual = svc.GetByGroupId(id);
            
            // Assert
            foreach (var order in actual)
            {
                Assert.Equal(expected.MealGroup.Id, order.MealGroup.Id);
            }
            
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetByIdTest(long id)
        {
            // Arrange
            var svc = SetupService();
            var dtos = GetDtoCollection();
            var expected = dtos.First(i => i.Id == id);

            // Act
            var actual = svc.Get(id);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.ImagePath, actual.ImagePath);
        }

        [Fact]
        public void GetByIdNotExistingTest()
        {
            // Arrange
            var svc = SetupService();

            // Act
            var actual = svc.Get(4);

            // Assert
            Assert.Null(actual);
        }
        
        [Fact]
        public void CreateTest()
        {
            // Arrange
            var repository = new Mock<IRepository<Meal, long>>();
            var commentRepository = new Mock<IRepository<Comment, long>>();
            var cart = new Mock<ICartService>();
            var mapper = new Mock<IMapper>();
            var svc = new MealService(repository.Object, commentRepository.Object, mapper.Object, _config.Object, cart.Object);
            var expected = new MealDTO()
            {
                Id = 4,
                Description = "desc",
                Name = "testMeal",
                MealGroup = new MealGroupDTO()
                {
                    Id = 5,
                    Name = "group5",
                }
            };

            // Act
            svc.Create(expected);

            // Assert
            mapper.Verify(m => m.Map<MealDTO, Meal>(It.IsAny<MealDTO>()), Times.Once());
            repository.Verify(r => r.Create(It.IsAny<Meal>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
        [Fact]
        public void UpdateTest()
        {
            // Arrange
            var expected = new MealDTO()
            {
                Id = 1,
                Name = "Anton"
            };
            var repository = new Mock<IRepository<Meal, long>>();
            var commentRepository = new Mock<IRepository<Comment, long>>();
            var cart = new Mock<ICartService>();
            repository.Setup(r => r.Get(expected.Id)).Returns(new Meal()
            {
                Id = 1,
                Name = "Anton"
            });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Meal, MealDTO>(It.IsAny<Meal>())).Returns(expected);
            var svc = new MealService(repository.Object, commentRepository.Object, mapper.Object, _config.Object, cart.Object);


            // Act
            svc.Update(expected);

            // Assert
            mapper.Verify(m => m.Map<MealDTO, Meal>(It.IsAny<MealDTO>()), Times.Once());
            repository.Verify(r => r.Update(It.IsAny<Meal>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var expected = new MealDTO()
            {
                Id = 1,
                Name = "Ivanko"
            };
            var repository = new Mock<IRepository<Meal, long>>();
            var cart = new Mock<ICartService>();
            repository.Setup(r => r.Get(expected.Id)).Returns(new Meal()
            {
                Id = 1,
                Name = "Ivanko"
            });
            var commentRepository = new Mock<IRepository<Comment, long>>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Meal, MealDTO>(It.IsAny<Meal>())).Returns(expected);
            var svc = new MealService(repository.Object, commentRepository.Object, mapper.Object, _config.Object, cart.Object);

            // Act
            svc.Delete(expected.Id);

            // Assert
            repository.Verify(r => r.Delete(It.IsAny<long>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
        private MealService SetupService()
        {
            var collection = GetMealCollection();
            var list = collection.AsQueryable();
            var mockSet = new Mock<DbSet<Meal>>();
            mockSet.As<IQueryable<Meal>>().Setup(p => p.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Meal>>().Setup(p => p.Expression).Returns(list.Expression);
            mockSet.As<IQueryable<Meal>>().Setup(p => p.ElementType).Returns(list.ElementType);
            mockSet.As<IQueryable<Meal>>().Setup(p => p.GetEnumerator()).Returns(list.GetEnumerator);

            var mockCtx = new Mock<ApplicationDbContext>();
            
            mockCtx.Setup( p=>p.Meals).Returns(mockSet.Object);

            var repository = new MealRepository(mockCtx.Object);
            var commentRepository = new CommentRepository(mockCtx.Object);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Meal, MealDTO>();
            });
            var mapper = mapperConfig.CreateMapper();
            var cart = new Mock<ICartService>();
            var svc = new MealService(repository, commentRepository, mapper, _config.Object, cart.Object);
            return svc;
        }
        
        
        private IEnumerable<MealDTO> GetDtoCollection()
        {
            return new[]
            {
                new MealDTO()
                {
                    Id = 1,
                    Name = "Meal1",
                    MealGroup = new MealGroupDTO()
                    {
                        Id = 1,
                        Name = "1"
                    },
                    ImagePath = "MealPics/1.jpg"
                },
                
                new MealDTO()
                {
                    Id = 2,
                    Name = "Meal2",
                    MealGroup = new MealGroupDTO()
                    {
                        Id = 2,
                        Name = "2"
                    },
                    ImagePath = "MealPics/1.jpg"
                },
                
                new MealDTO()
                {
                    Id = 3,
                    Name = "Meal3",
                    MealGroup = new MealGroupDTO()
                    {
                        Id = 3,         
                        Name = "3"
                    },
                    ImagePath = "MealPics/1.jpg"
                },

            };
            
        }

        private IEnumerable<Meal> GetMealCollection()
        {
            return new[]
            {
                new Meal()
                {
                    Id = 1,
                    Name = "Meal1",
                    MealGroup = new MealGroup()
                    {
                        Id = 1,
                        Name = "1"
                    },
                    ImagePath = "MealPics/1.jpg"

                },
                new Meal()
                {
                    Id = 2,
                    Name = "Meal2",
                    MealGroup = new MealGroup()
                    {
                        Id = 2,
                        Name = "2"
                    },
                    ImagePath = "MealPics/1.jpg"
                },
                new Meal()
                {
                    Id = 3,
                    Name = "Meal3",
                    MealGroup = new MealGroup()
                    {
                        Id = 3,
                        Name = "3"
                    },
                    ImagePath = "MealPics/1.jpg"
                }
            };
        }

    }
}