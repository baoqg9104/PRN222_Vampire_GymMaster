﻿using Core.Interfaces;
using Core.Response;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implements
{
    public class WorkoutSessionRepository : IWorkoutSessionRepository
    {
        private readonly GymManagementContext _context;

        public WorkoutSessionRepository(GymManagementContext context) { _context = context; }

        public async Task<WorkoutSession> AddAsync(WorkoutSession workoutSession)
        {
            if (workoutSession == null)
            {
                throw new ArgumentNullException(nameof(workoutSession), "Workout Session cannot be null");
            }
            _context.WorkoutSessions.Add(workoutSession);
            await _context.SaveChangesAsync();
            return workoutSession;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workoutSession = await GetByIdAsync(id);
            if (workoutSession == null)
            {
                return false;
            }
            _context.WorkoutSessions.Remove(workoutSession);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkoutSession>> GetAllAsync()
        {
            return await _context.WorkoutSessions.ToListAsync();
        }

        public async Task<WorkoutSession?> GetByIdAsync(int id)
        {
            return await _context.WorkoutSessions
                .Include(bm => bm.Plan)
                .Include(bm => bm.Member)
                .FirstOrDefaultAsync(bm => bm.SessionId == id);
        }

        public async Task<IEnumerable<WorkoutSession>> GetByPlanIdAsync(int PlanId)
        {
            return await _context.WorkoutSessions
                .Where(bm => bm.PlanId == PlanId)
                .ToListAsync();
        }

        public async Task<WorkoutSession> UpdateAsync(WorkoutSession workoutSession)
        {
            if (workoutSession == null)
            {
                throw new ArgumentNullException(nameof(workoutSession), "Workout Session cannot be null");
            }
            _context.WorkoutSessions.Update(workoutSession);
            await _context.SaveChangesAsync();
            return workoutSession;
        }

        public async Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(int userId)
        {
            return await _context.WorkoutSessions
                .Where(bm => bm.MemberId == userId)
                .Include(bm => bm.Member)
                .ToListAsync();
        }

        public async Task<WorkoutSessionResponse> GetListAsync(string? searchTypeName, int? id, int pageIndex, int pageSize)
        {
            var query = _context.WorkoutSessions.Include(p => p.Plan).Include(p => p.Member).AsQueryable();

            if (!string.IsNullOrEmpty(searchTypeName))
            {
                query = query.Where(p => p.Plan.ExerciseName.Contains(searchTypeName));
            }

            if (id.HasValue)
            {
                query = query.Where(p => p.MemberId == id.Value);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var session = await query
                .OrderByDescending(p => p.SessionId) // Optional: sort mới nhất lên đầu
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new WorkoutSessionResponse
            {
                WorkoutSession = session,
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
        }
    }
}
