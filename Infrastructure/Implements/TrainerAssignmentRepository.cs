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
           .Include(bm => bm.WorkoutPlans)
           .FirstOrDefaultAsync(bm => bm.AssignmentId == id);
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
