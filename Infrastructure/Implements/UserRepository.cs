using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly GymManagementContext _context;

        public UserRepository(GymManagementContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
           .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
            .FirstOrDefaultAsync(bp => bp.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
            .FirstOrDefaultAsync(bp => bp.UserId == id);
        }

       

        public async Task<User> UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public User Login(string email, string password)
        {

            // Check if the email and password match an existing account
            var lionAccount = _context.Users.FirstOrDefault(x => x.Email == email && x.PasswordHash == password);
            return lionAccount;
        }
    }
}
