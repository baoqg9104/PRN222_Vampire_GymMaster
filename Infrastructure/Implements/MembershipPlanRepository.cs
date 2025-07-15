using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implements;

public class MembershipPlanRepository : IMembershipPlanRepository
{
    private readonly GymManagementContext _context;

    public MembershipPlanRepository(GymManagementContext context) { _context = context; }

    public async Task<MembershipPlan> AddAsync(MembershipPlan membershipPlan)
    {
        if (membershipPlan == null)
        {
            throw new ArgumentNullException(nameof(membershipPlan), "Trainer assignment cannot be null");
        }
        _context.MembershipPlans.Add(membershipPlan);
        await _context.SaveChangesAsync();
        return membershipPlan;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var membershipPlan = await GetByIdAsync(id);
        if (membershipPlan == null)
        {
            return false;
        }
        _context.MembershipPlans.Remove(membershipPlan);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MembershipPlan>> GetAllAsync()
    {
        return await _context.MembershipPlans
           .Include(bm => bm.UserMemberships)
           .ToListAsync();
    }

    public async Task<MembershipPlan?> GetByIdAsync(int id)
    {
        return await _context.MembershipPlans
           .Include(bm => bm.UserMemberships)
           .FirstOrDefaultAsync(bm => bm.PlanId == id);
    }

    public async Task<IEnumerable<MembershipPlan>> GetByPlanIdAsync(int planId)
    {
        return await _context.MembershipPlans
            .Where(bm => bm.PlanId == planId)
            .Include(bm => bm.UserMemberships)
            .ToListAsync();
    }

    public async Task<MembershipPlan> UpdateAsync(MembershipPlan membershipPlan)
    {
        if (membershipPlan == null)
        {
            throw new ArgumentNullException(nameof(membershipPlan), "Body measurement cannot be null");
        }
        _context.MembershipPlans.Update(membershipPlan);
        await _context.SaveChangesAsync();
        return membershipPlan;
    }
}
