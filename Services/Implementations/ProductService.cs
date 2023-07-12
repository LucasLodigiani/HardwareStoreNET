using AutoMapper;
using Common.Dtos;
using Common.Entities;
using Data.Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<ProductDto>> GetAllProducts()
        {
            List<Product> products = await _repository.GetAllProductsAsync();

            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(products);

            return productsDto;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            Product? product = await _repository.FindProductByIdAsync(id);
            if(product == null)
            {
                return null;
            }

            ProductDto productDto = _mapper.Map<ProductDto>(product);

            return productDto;
            
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            Product? productExists = await _repository.FindProductByNameAsync(productDto.Name);
            if (productExists != null)
            {
                throw new Exception("El producto que estas intentando crear ya existe");
            }

            Product newProduct = _mapper.Map<Product>(productDto);

            int productId = await _repository.CreateProduct(newProduct);

            productDto.Id = productId;

            return productDto;

        }

        public async Task<Boolean> UpdateProduct(ProductDto productDto)
        {
            Product? product = await _repository.FindProductByIdAsync(productDto.Id);
            if (product == null)
            {
                throw new Exception("No se ha encontrado el producto que deseas modificar");
            }

            _mapper.Map(productDto, product);

            bool result = await _repository.UpdateProduct(product);

            if(result)
            {
                return true;
            }
            else
            {
                throw new Exception("Ha ocurrido un error al modificar este producto");
            }

        }

        public Task<bool> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
