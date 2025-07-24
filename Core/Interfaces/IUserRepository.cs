using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        User Login(string email, string password);
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetTrainerCountAsync();
        Task<int> GetMemberCountAsync();
        Task<int> GetNewUsersThisMonthAsync();
    }
}
