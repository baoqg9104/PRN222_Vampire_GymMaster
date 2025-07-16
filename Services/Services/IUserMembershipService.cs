using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IUserMembershipService
    {
        Task<IEnumerable<UserMembership>> GetAllAsync();
        Task<UserMembership?> GetByIdAsync(int id);
        Task<UserMembership> AddAsync(UserMembership user);
        Task<UserMembership> UpdateAsync(UserMembership user);
        Task<bool> DeleteAsync(int id);
    }
}
