using Core.Interfaces;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services;

public class MembershipPlanService : IMembershipPlanService
{
    private readonly IMembershipPlanRepository _membershipPlanService;
    public MembershipPlanService(IMembershipPlanRepository membershipPlanService) { _membershipPlanService = membershipPlanService; }
    public async Task<MembershipPlan> AddAsync(MembershipPlan membershipPlan)
    {
        return await _membershipPlanService.AddAsync(membershipPlan);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _membershipPlanService.DeleteAsync(id);
    }

    public async Task<IEnumerable<MembershipPlan>> GetAllAsync()
    {
        return await _membershipPlanService.GetAllAsync();
    }

    public async Task<MembershipPlan?> GetByIdAsync(int id)
    {
        return await _membershipPlanService.GetByIdAsync(id);
    }

    public async Task<IEnumerable<MembershipPlan>> GetByPlanIdAsync(int planId)
    {
        return await _membershipPlanService.GetByPlanIdAsync(planId);
    }

    public async Task<MembershipPlan> UpdateAsync(MembershipPlan membershipPlan)
    {
        return await _membershipPlanService.UpdateAsync(membershipPlan);
    }

    // Implementation in MembershipPlanService
    public async Task<int> GetActiveMembershipsCountAsync()
    {
        return await _membershipPlanService.GetActiveMembershipsCountAsync();
    }

    public async Task<int> GetTotalMembershipPlansCountAsync()
    {
        return await _membershipPlanService.GetTotalMembershipPlansCountAsync();
    }

}
