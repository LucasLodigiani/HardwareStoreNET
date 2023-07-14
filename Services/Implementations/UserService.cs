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

        public async Task<bool> DeleteUser(Guid id)
        {
            User? user = await _userRepository.FindUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            bool result = await _userRepository.DeleteUser(user);

            if(result)
            {
                return true;
            }
            else
            {
                throw new Exception("Ha ocurrido un error al eliminar al usuario");
            }
        }

        public async Task<IList<UserDto>> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllUsersAsync();

            List<UserDto> usersDto = _mapper.Map<List<UserDto>>(users);

            return usersDto;

        }

        public async Task<UserDto?> GetUserById(Guid id)
        {
            User? user = await _userRepository.FindUserByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;

        }

        public async Task<bool> UpdateUser(UserDto userDto)
        {
            User? user = await _userRepository.FindUserByIdAsync(userDto.Id);
            if (user == null)
            {
                return false;
            }

            _mapper.Map(userDto, user);

            bool result = await _userRepository.UpdateUser(user);

            if (result)
            {
                return true;
            }
            else
            {
                throw new Exception("Ha ocurrido un error al modificar al usuario");
            }


        }
    }
}
