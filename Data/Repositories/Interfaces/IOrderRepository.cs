using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {

        Task<int> NewOrder(Order order);

        Task<Boolean> UpdateOrder(Order order);

        Task<Order> FindOrderByIdAsync(int Id);

        Task<List<Order>> GetAllOrdersAsync();

    }
}
