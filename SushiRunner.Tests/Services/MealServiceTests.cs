using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using SushiRunner.Data;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services;
using SushiRunner.Services.Dto;
using Xunit;

namespace SushiRunner.Tests.Services
{
    public class MealServiceTests
    {
        
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
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetByGroupTest(string groupName)
        {
            // Arrange
            var list = GetDtoCollection();
            var svc = SetupService();
            var mealDtos = list.ToList();
            var expected = mealDtos.First(dto => dto.MealGroup.Name.Equals(groupName));
            
            // Act
            var mealDto = new MealDTO() {MealGroup = new MealGroup() {Name = groupName}};
            var actual = svc.GetByGroup(mealDto);
            
            // Assert
            foreach (var order in actual)
            {
                Assert.Equal(expected.MealGroup.Name, order.MealGroup.Name);
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

            var mapper = new Mock<IMapper>();
            var svc = new MealService(repository.Object, mapper.Object);
            var expected = new MealDTO()
            {
                Id = 4,
                Description = "desc",
                Name = "testMeal",
                MealGroup = new MealGroup()
                {
                    Id = 5,
                    Name = "group5"
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
            repository.Setup(r => r.Get(expected.Id)).Returns(new Meal()
            {
                Id = 1,
                Name = "Anton"
            });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Meal, MealDTO>(It.IsAny<Meal>())).Returns(expected);
            var svc = new MealService(repository.Object, mapper.Object);


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
            repository.Setup(r => r.Get(expected.Id)).Returns(new Meal()
            {
                Id = 1,
                Name = "Ivanko"
            });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Meal, MealDTO>(It.IsAny<Meal>())).Returns(expected);
            var svc = new MealService(repository.Object, mapper.Object);

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

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Meal, MealDTO>();
            });
            var mapper = mapperConfig.CreateMapper();
            var svc = new MealService(repository, mapper);
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
                    MealGroup = new MealGroup()
                    {
                        Id = 1,
                        Description = "Desc",
                        Name = "1"
                    }
                },
                
                new MealDTO()
                {
                    Id = 2,
                    Name = "Meal2",
                    MealGroup = new MealGroup()
                    {
                        Id = 2,
                        Description = "Desc",
                        Name = "2"
                    }
                },
                
                new MealDTO()
                {
                    Id = 3,
                    Name = "Meal3",
                    MealGroup = new MealGroup()
                    {
                        Id = 3,
                        Description = "Desc",
                        Name = "3"
                    }
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
                        Description = "Desc",
                        Name = "1"
                    }

                },
                new Meal()
                {
                    Id = 2,
                    Name = "Meal2",
                    MealGroup = new MealGroup()
                    {
                        Id = 1,
                        Description = "Desc",
                        Name = "2"
                    }
                },
                new Meal()
                {
                    Id = 3,
                    Name = "Meal3",
                    MealGroup = new MealGroup()
                    {
                        Id = 1,
                        Description = "Desc",
                        Name = "3"
                    }
                }
            };
        }

    }
}