using DTO.Models;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Interfaces
{
    public interface IOrderService : ICrudService<OrderDTO, long>
    {
        
    }
}