using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IWorkoutSessionRepository
    {
        Task<IEnumerable<WorkoutSession>> GetAllAsync();
        Task<WorkoutSession?> GetByIdAsync(int id);
        Task<IEnumerable<WorkoutSession>> GetByPlanIdAsync(int PlanId);
        Task<WorkoutSession> AddAsync(WorkoutSession workoutSession);
        Task<WorkoutSession> UpdateAsync(WorkoutSession workoutSession);
        Task<bool> DeleteAsync(int id);
    }
}
