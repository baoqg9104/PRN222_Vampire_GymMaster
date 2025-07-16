using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services;

public interface ITrainerAssignmentService
{
    Task<IEnumerable<TrainerAssignment>> GetAllAsync();
    Task<TrainerAssignment?> GetByIdAsync(int id);
    Task<IEnumerable<TrainerAssignment>> GetByTrainerIdAsync(int trainerId);
    Task<TrainerAssignment> AddAsync(TrainerAssignment trainerAssignment);
    Task<TrainerAssignment> UpdateAsync(TrainerAssignment trainerAssignment);
    Task<bool> DeleteAsync(int id);
}
