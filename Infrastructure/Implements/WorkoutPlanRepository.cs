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
    public class WorkoutPlanRepository : IWorkoutPlanRepository
    {
        private readonly GymManagementContext _context;

        public WorkoutPlanRepository(GymManagementContext context) { _context = context; }

        public async Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null)
            {
                throw new ArgumentNullException(nameof(workoutPlan), "Workout Plan cannot be null");
            }
            _context.WorkoutPlans.Add(workoutPlan);
            await _context.SaveChangesAsync();
            return workoutPlan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workoutPlan = await GetByIdAsync(id);
            if (workoutPlan == null)
            {
                return false;
            }
            _context.WorkoutPlans.Remove(workoutPlan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkoutPlan>> GetAllAsync()
        {
            return await _context.WorkoutPlans
                .Include(bm => bm.WorkoutSessions)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkoutPlan>> GetByAssignmentIdAsync(int AssignmentId)
        {
            return await _context.WorkoutPlans
                .Where(bm => bm.AssignmentId == AssignmentId)
                .Include(bm => bm.WorkoutSessions)
                .ToListAsync();
        }

        public async Task<WorkoutPlan?> GetByIdAsync(int id)
        {
            return await _context.WorkoutPlans
                .Include(bm => bm.WorkoutSessions)
               .FirstOrDefaultAsync(bm => bm.PlanId == id);
        }

        public async Task<WorkoutPlan> UpdateAsync(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null)
            {
                throw new ArgumentNullException(nameof(workoutPlan), "Workout Plan cannot be null");
            }
            _context.WorkoutPlans.Update(workoutPlan);
            await _context.SaveChangesAsync();
            return workoutPlan;
        }
    }
}
