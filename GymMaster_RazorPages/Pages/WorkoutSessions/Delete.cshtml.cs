using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Trainer")]

    public class DeleteModel : PageModel
    {
        private readonly IWorkoutSessionService _workoutSessionService;

        public DeleteModel(IWorkoutSessionService workoutSessionService)
        {
            _workoutSessionService = workoutSessionService;
        }

        [BindProperty]
        public WorkoutSession WorkoutSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutsession = await _workoutSessionService.GetByIdAsync(id.Value);

            if (workoutsession is not null)
            {
                WorkoutSession = workoutsession;

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

            var workoutsession = await _workoutSessionService.GetByIdAsync(id.Value);
            if (workoutsession != null)
            {
                WorkoutSession = workoutsession;
                await _workoutSessionService.DeleteAsync(WorkoutSession.SessionId);
            }

            return RedirectToPage("./Index");
        }
    }
}
