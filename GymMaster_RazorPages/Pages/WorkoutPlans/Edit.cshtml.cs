using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.WorkoutPlans
{
    public class EditModel : PageModel
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly ITrainerAssignmentService _trainerAssignmentService;

        public EditModel(IWorkoutPlanService workoutPlanService, ITrainerAssignmentService trainerAssignmentService)
        {
            _workoutPlanService = workoutPlanService;
            _trainerAssignmentService = trainerAssignmentService;
        }

        [BindProperty]
        public WorkoutPlan WorkoutPlan { get; set; } = default!;

        public SelectList? List { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutplan = await _workoutPlanService.GetByIdAsync(id.Value);
            if (workoutplan == null)
            {
                return NotFound();
            }
            WorkoutPlan = workoutplan;
            var list = await _trainerAssignmentService.GetAllAsync();
            List = new SelectList(list, "AssignmentId", "AssignmentId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}


            try
            {
                await _workoutPlanService.UpdateAsync(WorkoutPlan);
            }
            catch (Exception)
            {
                return NotFound(); // or return custom error page
            }

            return RedirectToPage("./Index");
        }
    }
}
