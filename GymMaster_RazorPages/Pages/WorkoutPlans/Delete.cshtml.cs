using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.WorkoutPlans
{
    [Authorize(Roles = "Trainer")]

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

            WorkoutPlan = await _workoutPlanService.GetByIdAsync(id.Value);

            if (WorkoutPlan == null)
            {
                return NotFound();
            }

            // Verify the current user is the trainer who owns this plan
            var trainerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (WorkoutPlan.Assignment.TrainerId != trainerId)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _workoutPlanService.GetByIdAsync(id.Value);
            if (plan == null)
            {
                return NotFound();
            }

            await _workoutPlanService.DeleteAsync(id.Value);

            return RedirectToPage("/Dashboard/TrainerDashboard");
        }
    }
}
