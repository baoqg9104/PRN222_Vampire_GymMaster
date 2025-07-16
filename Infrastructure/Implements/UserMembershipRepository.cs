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
    public class UserMembershipRepository : IUserMembershipRepository
    {
        private readonly GymManagementContext _context;

        public UserMembershipRepository(GymManagementContext context)
        {
            _context = context;
        }

        public async Task<UserMembership> AddAsync(UserMembership userMem)
        {
            if (userMem == null)
            {
                throw new ArgumentNullException(nameof(userMem), "User Membership cannot be null");
            }
            _context.UserMemberships.Add(userMem);
            await _context.SaveChangesAsync();
            return userMem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.UserMemberships.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserMembership>> GetAllAsync()
        {
            return await _context.UserMemberships
           .ToListAsync();
        }

        public async Task<UserMembership?> GetByIdAsync(int id)
        {
            return await _context.UserMemberships
            .FirstOrDefaultAsync(bp => bp.MembershipId == id);
        }

        public async Task<UserMembership> UpdateAsync(UserMembership user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User Membership cannot be null");
            }
            _context.UserMemberships.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
