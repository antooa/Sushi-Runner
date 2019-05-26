using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using SushiRunner.Data;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.MappingProfiles;
using SushiRunner.Services;
using SushiRunner.Services.Dto;
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
        [InlineData(OrderStatus.New)]
        [InlineData(OrderStatus.InProgress)]
        [InlineData(OrderStatus.Completed)]
        public void GetByStatusTest(OrderStatus status)
        {
            // Arrange
            var list = GetDtoCollection();
            var svc = SetupService();
            var orderDtos = list.ToList();
            var expected = orderDtos.First(o => o.OrderStatus.Equals(status));
            
            // Act
            var actual = svc.GetByStatus(status);
            
            // Assert
            foreach (var order in actual)
            {
                Assert.Equal(expected.OrderStatus, order.OrderStatus);
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
            Assert.Equal(expected.Address, actual.Address);
            Assert.Equal(expected.Items.Count(), actual.Items.Count());
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
            var repository = new Mock<IRepository<Order, long>>();

            var mapper = new Mock<IMapper>();
            var svc = new OrderService(repository.Object, mapper.Object);
            var expected = new OrderDTO
            {
                Id = 4,
                CustomerName = "Jim",
                Items = new List<OrderItemDTO>
                {
                    new OrderItemDTO()
                    {
                        Amount = 1,
                        Id = 4,
                        Meal = new MealDTO()
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
            mapper.Verify(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>()), Times.Once());
            repository.Verify(r => r.Create(It.IsAny<Order>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
        [Fact]
        public void UpdateTest()
        {
            // Arrange
            var expected = new OrderDTO()
            {
                Id = 1,
                CustomerName = "Ivanko"
            };
            var repository = new Mock<IRepository<Order, long>>();
            repository.Setup(r => r.Get(expected.Id)).Returns(new Order
            {
                Id = 1,
                CustomerName = "Ivanko"
            });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Order, OrderDTO>(It.IsAny<Order>())).Returns(expected);
            var svc = new OrderService(repository.Object, mapper.Object);


            // Act
            svc.Update(expected);

            // Assert
            mapper.Verify(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>()), Times.Once());
            repository.Verify(r => r.Update(It.IsAny<Order>()), Times.Once());
            repository.Verify(r => r.Save(), Times.Once());
        }
        
        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var expected = new OrderDTO
            {
                Id = 1,
                CustomerName = "Ivanko"
            };
            var repository = new Mock<IRepository<Order, long>>();
            repository.Setup(r => r.Get(expected.Id)).Returns(new Order
            {
                Id = 1,
                CustomerName = "Ivanko"
            });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Order, OrderDTO>(It.IsAny<Order>())).Returns(expected);
            var svc = new OrderService(repository.Object, mapper.Object);

            // Act
            svc.Delete(expected.Id);

            // Assert
            repository.Verify(r => r.Delete(It.IsAny<long>()), Times.Once());
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
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
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
                    Items = new List<OrderItemDTO>()
                    {
                        new OrderItemDTO()
                        {
                            Amount = 3,
                            Id = 1,
                            Meal = new MealDTO()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.InProgress
                },
                
                new OrderDTO()
                {
                    Id = 2,
                    CustomerName = "Arsen",
                    Address = "33 March 8th St., Uzhgorod, Ukraine",
                    DeliveredAt = DateTime.Now,
                    Items = new List<OrderItemDTO>()
                    {
                        new OrderItemDTO()
                        {
                            Amount = 2,
                            Id = 2,
                            Meal = new MealDTO()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.New
                },
                
                new OrderDTO()
                {
                    Id = 3,
                    CustomerName = "Donald",
                    Address = "347 Fifth Avenue, St. 1009, New York, NY",
                    PlacedAt = DateTime.Now,
                    DeliveredAt = (DateTime.Now).AddHours(1),
                    Items = new List<OrderItemDTO>()
                    {
                        new OrderItemDTO()
                        {
                            Amount = 3,
                            Id = 3,
                            Meal = new MealDTO()
                            {
                                Id = 1,
                                Description = "Trapeza",
                                Name = "Salo",
                                Price = 100,
                                MealGroup = new MealGroup()
                                {
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
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.InProgress
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
                                    Id = 1,
                                    Name = "Ukrainian"
                                },
                                Weight = 500
                            }
                        }
                    },
                    OrderStatus = OrderStatus.New
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
