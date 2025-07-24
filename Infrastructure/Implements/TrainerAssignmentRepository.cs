using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implements;

public class TrainerAssignmentRepository: ITrainerAssignmentRepository  
{
    private readonly GymManagementContext _context;

    public TrainerAssignmentRepository(GymManagementContext context) {  _context = context; }

    public async Task<User> GetCurrentTrainerAsync(int userId)
    {
        // Get the most recent active trainer assignment for the member
        var currentAssignment = await _context.TrainerAssignments
            .Include(ta => ta.Trainer) // Include the Trainer navigation property
            .Where(ta => ta.MemberId == userId &&
                        ta.IsActive == true &&
                        (ta.EndDate == null || ta.EndDate >= DateOnly.FromDateTime(DateTime.Today)))
            .OrderByDescending(ta => ta.StartDate)
            .FirstOrDefaultAsync();

        // Return the Trainer User object if assignment exists
        return currentAssignment?.Trainer;
    }
    public async Task<TrainerAssignment> AddAsync(TrainerAssignment trainerAssignment)
    {
        if(trainerAssignment == null) 
        {  
            throw new ArgumentNullException(nameof(trainerAssignment), "Trainer assignment cannot be null");
        }
        _context.TrainerAssignments.Add(trainerAssignment);
        await _context.SaveChangesAsync();
        return trainerAssignment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var trainerAssignment = await GetByIdAsync(id);
        if (trainerAssignment == null)
        {
            return false;
        }
        _context.TrainerAssignments.Remove(trainerAssignment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TrainerAssignment>> GetAllAsync()
    {
        return await _context.TrainerAssignments
           .Include(bm => bm.WorkoutPlans)
           .ToListAsync();
    }

    public async Task<TrainerAssignment?> GetByIdAsync(int id)
    {
        return await _context.TrainerAssignments
           .Include(ta => ta.Trainer) // Include Trainer
        .Include(ta => ta.Member)  // Include Member
        .Include(ta => ta.Membership) // Include Membership
        .Include(ta => ta.WorkoutPlans)
        .FirstOrDefaultAsync(ta => ta.AssignmentId == id);
    }

    public async Task<IEnumerable<TrainerAssignment>> GetByTrainerIdAsync(int trainerId)
    {
        return await _context.TrainerAssignments
            .Where(bm => bm.TrainerId == trainerId)
            .Include(bm => bm.WorkoutPlans)
            .ToListAsync();
    }

    public async Task<TrainerAssignment> UpdateAsync(TrainerAssignment trainerAssignment)
    {
        if (trainerAssignment == null)
        {
            throw new ArgumentNullException(nameof(trainerAssignment), "Body measurement cannot be null");
        }
        _context.TrainerAssignments.Update(trainerAssignment);
        await _context.SaveChangesAsync();
        return trainerAssignment;
    }

    
}
