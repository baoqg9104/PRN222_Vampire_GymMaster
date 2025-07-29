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
            var plan = await _context.WorkoutPlans
        .Include(p => p.WorkoutSessions)
        .FirstOrDefaultAsync(p => p.PlanId == id);

            if (plan == null)
            {
                return false;
            }

            // First delete all related sessions
            _context.WorkoutSessions.RemoveRange(plan.WorkoutSessions);

            // Then delete the plan
            _context.WorkoutPlans.Remove(plan);

            await _context.SaveChangesAsync();
            return true;

            //var workoutPlan = await GetByIdAsync(id);
            //if (workoutPlan == null)
            //{
            //    return false;
            //}
            //_context.WorkoutPlans.Remove(workoutPlan);
            //await _context.SaveChangesAsync();
            //return true;
        }

        public async Task<IEnumerable<WorkoutPlan>> GetAllAsync()
        {
            return await _context.WorkoutPlans
                .Include(bm => bm.Assignment)
                .ToListAsync();
        }


        public async Task<WorkoutPlanResponse> GetListAsync(string? searchTypeName, float? assignment, int pageIndex, int pageSize)
        {
            var query = _context.WorkoutPlans.Include(p => p.Assignment).AsQueryable();

            if (!string.IsNullOrEmpty(searchTypeName))
            {
                query = query.Where(p => p.ExerciseName.Contains(searchTypeName));
            }

            if (assignment.HasValue)
            {
                query = query.Where(p => p.AssignmentId == assignment.Value);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var plan = await query
                .OrderByDescending(p => p.PlanId) // Optional: sort mới nhất lên đầu
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new WorkoutPlanResponse
            {
                WorkoutPlan = plan,
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
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
                .Include(bm => bm.Assignment)
               .FirstOrDefaultAsync(bm => bm.PlanId == id);
        }

        public async Task<WorkoutPlan> UpdateAsync(WorkoutPlan workoutPlan)
        {
            _context.Entry(workoutPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return workoutPlan;
        }

        // WorkoutPlanRepository.cs
        public async Task<IEnumerable<WorkoutPlan>> GetByMemberIdAsync(int memberId)
        {
            return await _context.WorkoutPlans
                .Include(wp => wp.Assignment)
                .Where(wp => wp.Assignment.MemberId == memberId)
                .ToListAsync();
        }
    }
}
