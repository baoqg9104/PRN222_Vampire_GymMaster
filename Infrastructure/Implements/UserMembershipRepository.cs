using Core.Interfaces;
using Core.Response;
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
                        .Include(bm => bm.Plan)
                        .Include(bm => bm.User)
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

        public async Task<UserMembership> GetCurrentMembershipAsync(int userId)
        {
            return await _context.UserMemberships
                .Include(um => um.Plan) // Include the related MembershipPlan
                .Where(um => um.UserId == userId)
                .OrderByDescending(um => um.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<UserMembershipResponse> GetListAsync(string? searchTypeName, DateOnly? assignment, int pageIndex, int pageSize)
        {
            var query = _context.UserMemberships.Include(p => p.Plan).Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchTypeName))
            {
                query = query.Where(p => p.Plan.Name.Contains(searchTypeName));
            }

            if (assignment.HasValue)
            {
                query = query.Where(p => p.StartDate== assignment.Value);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var plan = await query
                .OrderByDescending(p => p.MembershipId) // Optional: sort mới nhất lên đầu
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new UserMembershipResponse
            {
                UserMembership = plan,
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
        }
    }
}
