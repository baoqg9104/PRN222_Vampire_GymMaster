using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.WorkoutSessions
{
    public class CreateModel : PageModel
    {
        private readonly IWorkoutSessionService _workoutSessionService;
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly IUserService _userService;

        public CreateModel(IWorkoutSessionService workoutSessionService, IWorkoutPlanService workoutPlanService, IUserService userService)
        {
            _workoutSessionService = workoutSessionService;
            _workoutPlanService = workoutPlanService;
            _userService = userService;
        }
        public SelectList MemberList { get; set; } = default!;
        public SelectList PlanList { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            MemberList = new SelectList(await _userService.GetAllAsync(), "UserId", "Email");
            PlanList = new SelectList(await _workoutPlanService.GetAllAsync(), "PlanId", "ExerciseName");
            return Page();
        }

        [BindProperty]
        public WorkoutSession WorkoutSession { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            await _workoutSessionService.AddAsync(WorkoutSession);

            return RedirectToPage("./Index");
        }
    }
}
