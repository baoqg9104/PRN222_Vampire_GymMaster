using Core.Interfaces;
using Core.Response;
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
        private readonly IWorkoutPlanRepository _workoutPlanService;
        public WorkoutPlanService(IWorkoutPlanRepository workoutPlanService) { _workoutPlanService = workoutPlanService; }
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

        public async Task<WorkoutPlanResponse> GetListAsync(string? searchTypeName, float? assignment, int pageIndex, int pageSize)
        {
            return await _workoutPlanService.GetListAsync(searchTypeName, assignment, pageIndex, pageSize);
        }    

        public async Task<WorkoutPlan> UpdateAsync(WorkoutPlan workoutPlan)
        {
            return await _workoutPlanService.UpdateAsync(workoutPlan);
        }

        public async Task<IEnumerable<WorkoutPlan>> GetByMemberIdAsync(int memberId)
        {   
            return await _workoutPlanService.GetByMemberIdAsync(memberId);
        }
    }
}
