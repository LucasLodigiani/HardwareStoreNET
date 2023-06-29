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
    public class RegisterProfile : Profile
    {
        public RegisterProfile() 
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
