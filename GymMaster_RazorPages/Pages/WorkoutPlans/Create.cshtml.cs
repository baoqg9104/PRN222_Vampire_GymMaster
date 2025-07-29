using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.WorkoutPlans
{
    [Authorize(Roles = "Trainer")]

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

        //public async Task<IActionResult> OnGet()
        //{
        //    var list = await _trainerAssignmentService.GetAllAsync();
        //    List = new SelectList(list, "AssignmentId", "AssignmentId");
        //    return Page();
        //}

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

        public async Task<IActionResult> OnGet(int? assignmentId)
        {
            // Get all assignments if no assignmentId specified
            var assignments = await _trainerAssignmentService.GetAllAsync();

            if (assignmentId.HasValue)
            {
                // Verify the assignment exists
                var assignmentExists = assignments.Any(a => a.AssignmentId == assignmentId.Value);
                if (!assignmentExists)
                {
                    return NotFound();
                }

                // Pre-set the assignmentId
                WorkoutPlan = new WorkoutPlan
                {
                    AssignmentId = assignmentId.Value
                };

                // Create SelectList with only the specified assignment
                List = new SelectList(
                    assignments.Where(a => a.AssignmentId == assignmentId.Value),
                    "AssignmentId",
                    "DisplayName",  // You'll need to add this property or use another meaningful display
                    assignmentId.Value);
            }
            else
            {
                // Create SelectList with all assignments
                List = new SelectList(assignments, "AssignmentId", "DisplayName");
            }

            return Page();
        }
    }
}
