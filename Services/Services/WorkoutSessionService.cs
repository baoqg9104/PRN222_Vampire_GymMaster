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
    public class WorkoutSessionervice : IWorkoutSessionService
    {
        private readonly IWorkoutSessionRepository _workoutSessionService;
        public WorkoutSessionervice(IWorkoutSessionRepository workoutSessionService) { _workoutSessionService = workoutSessionService; }

        public async Task<WorkoutSession> AddAsync(WorkoutSession workoutSession)
        {
            return await _workoutSessionService.AddAsync(workoutSession);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workoutSessionService.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkoutSession>> GetAllAsync()
        {
            return await _workoutSessionService.GetAllAsync();
        }

        public async Task<WorkoutSession?> GetByIdAsync(int id)
        {
            return await _workoutSessionService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<WorkoutSession>> GetByPlanIdAsync(int PlanId)
        {
            return await _workoutSessionService.GetByPlanIdAsync(PlanId);
        }

        public async Task<WorkoutSession> UpdateAsync(WorkoutSession workoutSession)
        {
            return await _workoutSessionService.UpdateAsync(workoutSession);
        }


        public async Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(int userId)
        {
            return await _workoutSessionService.GetByUserIdAsync(userId);
        }

        public async Task<WorkoutSessionResponse> GetListAsync(string? searchTypeName, int? id, int pageIndex, int pageSize)
        {
            return await _workoutSessionService.GetListAsync(searchTypeName, id, pageIndex, pageSize);
        }
    }
}
