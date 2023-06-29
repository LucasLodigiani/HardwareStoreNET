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
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<OrderUpdateDto, Order>();

            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => 1));

            CreateMap<OrderViewDto, Order>();

            CreateMap<Order, OrderViewDto>().ReverseMap();
        }
    }
}
