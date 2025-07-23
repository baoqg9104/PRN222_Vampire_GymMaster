using Core.Interfaces;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services;

public class TrainerAssignmentService : ITrainerAssignmentService
{
    private readonly ITrainerAssignmentRepository _trainerAssignmentService;
    public TrainerAssignmentService(ITrainerAssignmentRepository trainerAssignmentService) { _trainerAssignmentService = trainerAssignmentService; }
    public async Task<TrainerAssignment> AddAsync(TrainerAssignment trainerAssignment)
    {
        return await _trainerAssignmentService.AddAsync(trainerAssignment);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _trainerAssignmentService.DeleteAsync(id);
    }

    public async Task<IEnumerable<TrainerAssignment>> GetAllAsync()
    {
        return await _trainerAssignmentService.GetAllAsync();
    }

    public async Task<TrainerAssignment?> GetByIdAsync(int id)
    {
        return await _trainerAssignmentService.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TrainerAssignment>> GetByTrainerIdAsync(int trainerId)
    {
        return await _trainerAssignmentService.GetByTrainerIdAsync(trainerId);
    }

    public async Task<TrainerAssignment> UpdateAsync(TrainerAssignment trainerAssignment)
    {
        return await _trainerAssignmentService.UpdateAsync(trainerAssignment);
    }

    public Task<User> GetCurrentTrainerAsync(int userId)
    {
        return _trainerAssignmentService.GetCurrentTrainerAsync(userId);
    }
}
