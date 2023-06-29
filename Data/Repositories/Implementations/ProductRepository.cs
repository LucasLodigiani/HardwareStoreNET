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
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(ApplicationDbContext _context) : base(_context) { }

        public async Task<int> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            if(await SaveChangesAsync())
            {
                return product.Id;
            }
            else
            {
                throw new Exception("Ha ocurrido un error al guardar el nuevo producto");
            }

        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return (await _context.Products.ToListAsync());
        }

        public async Task<Product?> FindProductByIdAsync(int productId)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            return product;
        }

        public async Task<Product?> FindProductByNameAsync(string productName)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(n => n.Name == productName);

            return product;
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productsIds)
        {
            List<Product> products = await _context.Products.Where(p => productsIds.Contains(p.Id)).ToListAsync();

            return products;

        }
    }
}
