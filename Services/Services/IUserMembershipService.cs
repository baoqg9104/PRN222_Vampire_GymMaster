using Core.Response;
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
        Task<UserMembershipResponse> GetListAsync(string? searchTypeName, DateOnly? assignment, int pageIndex, int pageSize);
        Task<UserMembership?> GetByIdAsync(int id);
        Task<UserMembership> AddAsync(UserMembership user);
        Task<UserMembership> UpdateAsync(UserMembership user);
        Task<bool> DeleteAsync(int id);

        Task<UserMembership> GetCurrentMembershipAsync(int userId);

        Task<List<UserMembership>> GetMembershipsByUserIdAsync(int userId);

    }
}
