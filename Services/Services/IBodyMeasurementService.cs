using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSSQLServer.EntitiesModels;

namespace Services.Services;

public interface IBodyMeasurementService
{
    Task<IEnumerable<BodyMeasurement>> GetAllAsync();
    Task<BodyMeasurement?> GetByIdAsync(int id);
    Task<IEnumerable<BodyMeasurement>> GetByUserIdAsync(int userId);
    Task<BodyMeasurement> AddAsync(BodyMeasurement bodyMeasurement);
    Task<BodyMeasurement> UpdateAsync(BodyMeasurement bodyMeasurement);
    Task<bool> DeleteAsync(int id);
}
