using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.WorkoutPlans
{
    public class IndexModel : PageModel
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly ITrainerAssignmentService _trainerAssignmentService;

        public IndexModel(IWorkoutPlanService workoutPlanService, ITrainerAssignmentService trainerAssignmentService)
        {
            _workoutPlanService = workoutPlanService;
            _trainerAssignmentService = trainerAssignmentService;
        }
        [BindProperty(SupportsGet = true)]
        public string? searchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public float? assignmentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalPages { get; set; }

        public IList<WorkoutPlan> WorkoutPlan { get;set; } = default!;
        public IEnumerable<TrainerAssignment> TrainerAssignments { get;set; } = new List<TrainerAssignment>();

        public async Task OnGetAsync()
        {
            var result = await _workoutPlanService.GetListAsync(searchTerm, assignmentId, PageIndex, 6);
            WorkoutPlan = result.WorkoutPlan;
            TotalPages = result.TotalPages;
            PageIndex = result.PageIndex;

            TrainerAssignments = await _trainerAssignmentService.GetAllAsync();
        }
    }
}
