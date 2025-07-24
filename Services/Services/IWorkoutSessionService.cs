using Core.Response;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IWorkoutSessionService
    {
        Task<IEnumerable<WorkoutSession>> GetAllAsync();
        Task<WorkoutSessionResponse> GetListAsync(string? searchTypeName, int? id, int pageIndex, int pageSize);
        Task<WorkoutSession?> GetByIdAsync(int id);
        Task<IEnumerable<WorkoutSession>> GetByPlanIdAsync(int PlanId);
        Task<WorkoutSession> AddAsync(WorkoutSession workoutSession);
        Task<WorkoutSession> UpdateAsync(WorkoutSession workoutSession);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(int userId);
    }
}
