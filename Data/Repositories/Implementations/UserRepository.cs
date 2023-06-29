using Common.Entities;
using Data.DbContexts;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(ApplicationDbContext _context) : base(_context)
        {
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);

            return (await SaveChangesAsync());

        }

        public async Task<User?> FindUserByNameAsync(string username)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(name => name.Username == username);

            return user;

        }

        public async Task<bool> CheckPasswordAsync(string username,string password)
        {
            User? user = await FindUserByNameAsync(username);

            if(user == null)
            {
                throw new Exception("User not found");
            }

            if(user.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
