using Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IList<UserDto>> GetAllUsers();

        Task<Boolean> UpdateUser(UserDto userDto);

        Task<Boolean> DeleteUser(Guid id);

        Task<UserDto?> GetUserById(Guid id);

    }
}
