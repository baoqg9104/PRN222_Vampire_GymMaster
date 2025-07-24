using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.WorkoutSessions
{
    public class IndexModel : PageModel
    {
        private readonly IWorkoutSessionService _workoutSessionService;
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly IUserService _userService;

        public IndexModel(IWorkoutSessionService workoutSessionService, IWorkoutPlanService workoutPlanService, IUserService userService) { 
            _workoutSessionService = workoutSessionService;
            _workoutPlanService = workoutPlanService;
            _userService = userService;
        }
        [BindProperty(SupportsGet = true)]
        public string? searchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalPages { get; set; }

        public IList<WorkoutSession> WorkoutSession { get;set; } = default!;
        public IEnumerable<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
        public IEnumerable<User> Users { get; set; } = new List<User>();

        public async Task OnGetAsync()
        {
            var result = await _workoutSessionService.GetListAsync(searchTerm, id, PageIndex, 6);
            WorkoutSession = result.WorkoutSession;
            TotalPages = result.TotalPages;
            PageIndex = result.PageIndex;

            WorkoutPlans = await _workoutPlanService.GetAllAsync();
            Users = await _userService.GetAllAsync();
        }
    }
}
