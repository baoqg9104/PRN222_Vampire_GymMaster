using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface ITrainerAssignmentRepository
{
    Task<IEnumerable<TrainerAssignment>> GetAllAsync();
    Task<TrainerAssignment?> GetByIdAsync(int id);
    Task<IEnumerable<TrainerAssignment>> GetByTrainerIdAsync(int trainerId);
    Task<TrainerAssignment> AddAsync(TrainerAssignment trainerAssignment);
    Task<TrainerAssignment> UpdateAsync(TrainerAssignment trainerAssignment);
    Task<bool> DeleteAsync(int id);
    Task<User> GetCurrentTrainerAsync(int userId);

}
