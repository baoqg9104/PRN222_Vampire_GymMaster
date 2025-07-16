using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        public WorkoutPlanService(IWorkoutPlanService workoutPlanService) { _workoutPlanService = workoutPlanService; }
        public async Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan)
        {
            return await _workoutPlanService.AddAsync(workoutPlan);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workoutPlanService.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkoutPlan>> GetAllAsync()
        {
            return await _workoutPlanService.GetAllAsync();
        }

        public async Task<IEnumerable<WorkoutPlan>> GetByAssignmentIdAsync(int AssignmentId)
        {
            return await _workoutPlanService.GetByAssignmentIdAsync(AssignmentId);
        }

        public async Task<WorkoutPlan?> GetByIdAsync(int id)
        {
            return await _workoutPlanService.GetByIdAsync(id);
        }

        public async Task<WorkoutPlan> UpdateAsync(WorkoutPlan workoutPlan)
        {
            return await _workoutPlanService.UpdateAsync(workoutPlan);
        }
    }
}
