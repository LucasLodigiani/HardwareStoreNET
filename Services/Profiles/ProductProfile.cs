using AutoMapper;
using Common.Dtos;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>();

            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
