using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using System.Security.Claims;

namespace GymMaster_RazorPages.Pages.Dashboard
{
    public class TrainerDasboardModel : PageModel
    {
        private readonly GymManagementContext _context;

        public TrainerDasboardModel(GymManagementContext context)
        {
            _context = context;
        }

        public List<TrainerAssignmentWithDetails> TrainerAssignments { get; set; } = new();

        public class TrainerAssignmentWithDetails
        {
            public int AssignmentId { get; set; }
            public User Member { get; set; }
            public UserMembership Membership { get; set; }
            public List<WorkoutPlan> WorkoutPlans { get; set; }

            public int MemberId => Member.UserId;
        }

        public async Task OnGetAsync()
        {
            var trainerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            TrainerAssignments = await _context.TrainerAssignments
                .Include(ta => ta.Member)
                .Include(ta => ta.Membership)
                    .ThenInclude(um => um.Plan)
                .Include(ta => ta.WorkoutPlans)
                .Where(ta => ta.TrainerId == trainerId && (bool)ta.IsActive)
                .Select(ta => new TrainerAssignmentWithDetails
                {
                    AssignmentId = ta.AssignmentId,
                    Member = ta.Member,
                    Membership = ta.Membership,
                    WorkoutPlans = ta.WorkoutPlans.ToList()
                })
                .ToListAsync();
        }
    }
}
