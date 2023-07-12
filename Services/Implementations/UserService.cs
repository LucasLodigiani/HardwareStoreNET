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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IList<UserDto>> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllUsersAsync();

            List<UserDto> usersDto = _mapper.Map<List<UserDto>>(users);

            return usersDto;

        }
    }
}
