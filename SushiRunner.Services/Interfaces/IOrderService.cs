using System.Collections.Generic;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IOrderService : ICrudService<OrderDTO, long>
    {
        IEnumerable<OrderDTO> GetByStatus(OrderStatus status);

        void Create(User user, string customerName, string phoneNumber, string paymentType, string address,
            CartDTO cart);
    }
}