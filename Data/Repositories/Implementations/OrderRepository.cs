using Common.Entities;
using Data.DbContexts;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext _context) : base(_context) { }

        public async Task<int> NewOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            if(await SaveChangesAsync())
            {
                return order.Id;
            }
            else
            {
                throw new Exception("Ha ocurrido un error al generar la orden");
            }
        }

        public async Task<Order> FindOrderByIdAsync(int Id)
        {
            Order? order = await _context.Orders.Include(p => p.Products).FirstOrDefaultAsync(o => o.Id == Id);

            return order;
        }

        public async Task<Boolean> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);

            return (await SaveChangesAsync());
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            List<Order> orders = await _context.Orders.Include(p => p.Products).ToListAsync();

            return orders;
        }
    }
}
