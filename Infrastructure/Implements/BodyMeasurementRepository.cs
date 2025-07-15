using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace Infrastructure.Implements;

public class BodyMeasurementRepository : IBodyMeasurementRepository
{
    private readonly GymManagementContext _context;

    public BodyMeasurementRepository(GymManagementContext context)
    {
        _context = context;
    }

    public async Task<BodyMeasurement> AddAsync(BodyMeasurement bodyMeasurement)
    {
        if (bodyMeasurement == null)
        {
            throw new ArgumentNullException(nameof(bodyMeasurement), "Body measurement cannot be null");
        }
        _context.BodyMeasurements.Add(bodyMeasurement);
        await _context.SaveChangesAsync();
        return bodyMeasurement;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var bodyMeasurement = await GetByIdAsync(id);
        if (bodyMeasurement == null)
        {
            return false;
        }
        _context.BodyMeasurements.Remove(bodyMeasurement);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BodyMeasurement>> GetAllAsync()
    {
        return await _context.BodyMeasurements
            .Include(bm => bm.Member)
            .ToListAsync();
    }

    public async Task<BodyMeasurement?> GetByIdAsync(int id)
    {
        return await _context.BodyMeasurements
            .Include(bm => bm.Member)
            .FirstOrDefaultAsync(bm => bm.MeasurementId == id);
    }

    public async Task<IEnumerable<BodyMeasurement>> GetByUserIdAsync(int userId)
    {
        return await _context.BodyMeasurements
            .Where(bm => bm.MemberId == userId)
            .Include(bm => bm.Member)
            .ToListAsync();
    }

    public async Task<BodyMeasurement> UpdateAsync(BodyMeasurement bodyMeasurement)
    {
        if (bodyMeasurement == null)
        {
            throw new ArgumentNullException(nameof(bodyMeasurement), "Body measurement cannot be null");
        }
        _context.BodyMeasurements.Update(bodyMeasurement);
        await _context.SaveChangesAsync();
        return bodyMeasurement;
    }
}
