using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DTO.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using SushiRunner.Data;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services;
using Xunit;

namespace SushiRunner.Tests.Services
{
    public class OrderServiceTests
    {
        
        [Fact]
        public void GetListTest()
        {
            // Arrange
            var list = GetDtoCollection();
            var svc = SetupService();
            
            // Act
            IEnumerable<OrderDTO> actual = svc.GetList();
            
            // Assert
            Assert.Equal(actual.Count(), list.Count());

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
            Assert.Equal(expected.Address, actual.Address);
            Assert.Equal(expected.Items.Count, actual.Items.Count);
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
            var repository = new Mock<OrderRepository>(null);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = mapperConfig.CreateMapper();
            var svc = new OrderService(repository.Object, mapper);
            var expected = new OrderDTO
            {
                Id = 4,
                CustomerName = "Jim",
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Amount = 1,
                        Id = 4,
                        Meal = new Meal
                        {
                            Id = 2,
                            Price = 50,
                            Name = "burek"
                        }
                    }
                },
                PlacedAt = DateTime.Now
                
                
            };

            // Act
            svc.Create(expected);

            // Assert
            repository.Verify(r => r.Create(It.IsAny<Order>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
        private OrderService SetupService()
        {
            var collection = GetOrderCollection();
            var list = collection.AsQueryable();
            var mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(p => p.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Order>>().Setup(p => p.Expression).Returns(list.Expression);
            mockSet.As<IQueryable<Order>>().Setup(p => p.ElementType).Returns(list.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(p => p.GetEnumerator()).Returns(list.GetEnumerator);

            var mockCtx = new Mock<ApplicationDbContext>();
            
            mockCtx.Setup( p=>p.Orders).Returns(mockSet.Object);

            var repository = new OrderRepository(mockCtx.Object);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = mapperConfig.CreateMapper();
            var svc = new OrderService(repository, mapper);
            return svc;
        }
        
        
        private IEnumerable<OrderDTO> GetDtoCollection()
        {
            return new[]
            {
                new OrderDTO()
                {
                    Id = 1,
                    CustomerName = "Taras",
                    Address = "77/T77 Heroiv UPA St., Lviv, Ukraine",
                    DeliveredAt = DateTime.Now,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Amount = 3,
                            Id = 1,
                            Meal = new Meal()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Description = "best",
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.Cooking
                },
                
                new OrderDTO()
                {
                    Id = 2,
                    CustomerName = "Arsen",
                    Address = "33 March 8th St., Uzhgorod, Ukraine",
                    DeliveredAt = DateTime.Now,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Amount = 2,
                            Id = 2,
                            Meal = new Meal()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Description = "best",
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.Cooking
                },
                
                new OrderDTO()
                {
                    Id = 3,
                    CustomerName = "Donald",
                    Address = "347 Fifth Avenue, St. 1009, New York, NY",
                    PlacedAt = DateTime.Now,
                    DeliveredAt = (DateTime.Now).AddHours(1),
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Amount = 3,
                            Id = 3,
                            Meal = new Meal()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Description = "best",
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.Completed,                   
                },

            };
            
        }

        private IEnumerable<Order> GetOrderCollection()
        {
            return new[]
            {
                new Order()
                {
                    Id = 1,
                    CustomerName = "Taras",
                    Address = "77/T77 Heroiv UPA St., Lviv, Ukraine",
                    DeliveredAt = DateTime.Now,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Amount = 3,
                            Id = 1,
                            Meal = new Meal()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Description = "best",
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.Cooking
                },
                new Order()
                {
                    Id = 2,
                    CustomerName = "Arsen",
                    Address = "33 March 8th St., Uzhgorod, Ukraine",
                    DeliveredAt = DateTime.Now,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Amount = 2,
                            Id = 2,
                            Meal = new Meal()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Description = "best",
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.Cooking
                },
                new Order()
                {
                    Id = 3,
                    CustomerName = "Donald",
                    Address = "347 Fifth Avenue, St. 1009, New York, NY",
                    PlacedAt = DateTime.Now,
                    DeliveredAt = (DateTime.Now).AddHours(1),
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Amount = 3,
                            Id = 3,
                            Meal = new Meal()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Description = "best",
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.Completed, 
                }
            };
        }
        
    }
}