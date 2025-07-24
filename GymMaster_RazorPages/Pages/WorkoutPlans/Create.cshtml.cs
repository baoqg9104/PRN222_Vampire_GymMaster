using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.WorkoutPlans
{
    public class CreateModel : PageModel
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly ITrainerAssignmentService _trainerAssignmentService;

        public CreateModel(IWorkoutPlanService workoutPlanService, ITrainerAssignmentService trainerAssignmentService)
        {
            _workoutPlanService = workoutPlanService;
            _trainerAssignmentService = trainerAssignmentService;
        }
        public SelectList List { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            var list = await _trainerAssignmentService.GetAllAsync();
            List = new SelectList(list, "AssignmentId", "AssignmentId");
            return Page();
        }

        [BindProperty]
        public WorkoutPlan WorkoutPlan { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            await _workoutPlanService.AddAsync(WorkoutPlan);

            return RedirectToPage("./Index");
        }
    }
}
