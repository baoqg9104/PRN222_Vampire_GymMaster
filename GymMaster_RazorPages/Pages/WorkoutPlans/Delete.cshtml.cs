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
    public class DeleteModel : PageModel
    {
        private readonly IWorkoutPlanService _workoutPlanService;

        public DeleteModel(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [BindProperty]
        public WorkoutPlan WorkoutPlan { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutplan = await _workoutPlanService.GetByIdAsync(id.Value);

            if (workoutplan is not null)
            {
                WorkoutPlan = workoutplan;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutplan = await _workoutPlanService.GetByIdAsync(id.Value);
            if (workoutplan != null)
            {
                WorkoutPlan = workoutplan;
                await _workoutPlanService.DeleteAsync(WorkoutPlan.PlanId);
            }

            return RedirectToPage("./Index");
        }
    }
}
