using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using MSSQLServer.EntitiesModels;

namespace Services.Services;

public class BodyMeasurementService : IBodyMeasurementService
{
    private readonly IBodyMeasurementRepository _bodyMeasurementRepository;

    public BodyMeasurementService(IBodyMeasurementRepository bodyMeasurementRepository)
    {
        _bodyMeasurementRepository = bodyMeasurementRepository;
    }

    public async Task<BodyMeasurement> AddAsync(BodyMeasurement bodyMeasurement)
    {
        return await _bodyMeasurementRepository.AddAsync(bodyMeasurement);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _bodyMeasurementRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<BodyMeasurement>> GetAllAsync()
    {
        return await _bodyMeasurementRepository.GetAllAsync();
    }

    public async Task<BodyMeasurement?> GetByIdAsync(int id)
    {
        return await _bodyMeasurementRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<BodyMeasurement>> GetByUserIdAsync(int userId)
    {
        return await _bodyMeasurementRepository.GetByUserIdAsync(userId);
    }

    public async Task<BodyMeasurement> UpdateAsync(BodyMeasurement bodyMeasurement)
    {
        return await _bodyMeasurementRepository.UpdateAsync(bodyMeasurement);
    }
}
