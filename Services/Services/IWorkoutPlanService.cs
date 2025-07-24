using Core.Response;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IWorkoutPlanService
    {
        Task<IEnumerable<WorkoutPlan>> GetAllAsync();
        Task<WorkoutPlanResponse> GetListAsync(string? searchTypeName, float? assignment, int pageIndex, int pageSize);
        Task<WorkoutPlan?> GetByIdAsync(int id);
        Task<IEnumerable<WorkoutPlan>> GetByAssignmentIdAsync(int AssignmentId);
        Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan);
        Task<WorkoutPlan> UpdateAsync(WorkoutPlan workoutPlan);
        Task<bool> DeleteAsync(int id);
    }
}
