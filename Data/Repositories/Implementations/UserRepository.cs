﻿using Common.Entities;
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = await _context.Users.ToListAsync();
            if(users.Count < 1)
            {
                throw new Exception("No hay usuarios para listar");
            }
            else
            {
                return users;
            }
        }

        public async Task<bool> DeleteUser(User user)
        {
            _context.Users.Remove(user);

            return (await SaveChangesAsync());
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

            return user;
        }

        public async Task<Boolean> UpdateUser(User user)
        {
            _context.Users.Update(user);

            return (await SaveChangesAsync());
        }
    }
}
