using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace GymMaster_RazorPages.Pages.WorkoutSessions
{
    public class ClientProgressModel : PageModel
    {
        private readonly GymManagementContext _context;

        public User Member { get; set; }
        public List<WorkoutSessionWithPlan> Sessions { get; set; } = new();

        public class WorkoutSessionWithPlan
        {
            public WorkoutSession Session { get; set; }
            public WorkoutPlan Plan { get; set; }
        }

        public ClientProgressModel(GymManagementContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int memberId)
        {
            // Get member details
            Member = await _context.Users.FirstOrDefaultAsync(u => u.UserId == memberId);

            // Get all sessions with their associated plans
            Sessions = await _context.WorkoutSessions
                .Include(ws => ws.Plan)
                .Where(ws => ws.MemberId == memberId)
                .OrderByDescending(ws => ws.CompletedAt)
                .Select(ws => new WorkoutSessionWithPlan
                {
                    Session = ws,
                    Plan = ws.Plan
                })
                .ToListAsync();
        }
    }
}
