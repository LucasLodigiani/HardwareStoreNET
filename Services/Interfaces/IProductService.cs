﻿using Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateProduct(ProductDto productDto);

        Task<Boolean> UpdateProduct(ProductDto productDto);

        Task<Boolean> DeleteProduct(int id);

        Task<ProductDto> GetProductById(int id);

        Task<IList<ProductDto>> GetAllProducts();

    }
}
