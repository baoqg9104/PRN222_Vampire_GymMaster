using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IMembershipPlanRepository
{
    Task<IEnumerable<MembershipPlan>> GetAllAsync();
    Task<MembershipPlan?> GetByIdAsync(int id);
    Task<IEnumerable<MembershipPlan>> GetByPlanIdAsync(int planId);
    Task<MembershipPlan> AddAsync(MembershipPlan membershipPlan);
    Task<MembershipPlan> UpdateAsync(MembershipPlan membershipPlan);
    Task<bool> DeleteAsync(int id);
}
