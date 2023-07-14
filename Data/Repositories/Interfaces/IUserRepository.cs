using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindUserByNameAsync(string username);

        Task<User?> FindUserByIdAsync(Guid id);

        Task<List<User>> GetAllUsersAsync();

        Task<Boolean> CheckPasswordAsync(string username, string password);

        Task<Boolean> UpdateUser(User user);

        Task<Boolean> CreateUserAsync(User user);

        Task<Boolean> DeleteUser(User user);

    }
}
