using Common.Dtos;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderViewDto> CreateOrder(OrderCreateDto orderDto);

        Task<Boolean> UpdateOrder(OrderUpdateDto orderUpdateDto);

        Task<OrderViewDto?> GetOrderById(int id, Guid userId);

        Task<List<OrderViewDto>> GetAllOrders();
    }
}
