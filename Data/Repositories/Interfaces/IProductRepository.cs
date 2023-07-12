using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IProductRepository
    {

        Task<int> CreateProduct(Product product);

        Task<Boolean> UpdateProduct(Product product);

        Task<Product?> FindProductByNameAsync(string productName);

        Task<Product?> FindProductByIdAsync(int productId);

        Task<List<Product>> GetAllProductsAsync();

        Task<List<Product>> GetProductsByIdsAsync(List<int> productsIds);
    }
}
