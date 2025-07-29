using Microsoft.AspNetCore.Authorization;
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

namespace GymMaster_RazorPages.Pages.WorkoutSessions
{
    [Authorize(Roles = "Trainer")]

    public class EditModel : PageModel
    {
        private readonly IWorkoutSessionService _workoutSessionService;
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly IUserService _userService;

        public EditModel(IWorkoutSessionService workoutSessionService, IWorkoutPlanService workoutPlanService, IUserService userService)
        {
            _workoutSessionService = workoutSessionService;
            _workoutPlanService = workoutPlanService;
            _userService = userService;
        }

        [BindProperty]
        public WorkoutSession WorkoutSession { get; set; } = default!;
        public SelectList MemberList { get; set; } = default!;
        public SelectList PlanList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutsession =  await _workoutSessionService.GetByIdAsync(id.Value);
            if (workoutsession == null)
            {
                return NotFound();
            }
            WorkoutSession = workoutsession;
            MemberList = new SelectList(await _userService.GetAllAsync(), "UserId", "Email");
            PlanList = new SelectList(await _workoutPlanService.GetAllAsync(), "PlanId", "ExerciseName");
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

            //_context.Attach(WorkoutSession).State = EntityState.Modified;

            try
            {
                await _workoutSessionService.UpdateAsync(WorkoutSession);
            }
            catch (Exception)
            {
                return NotFound(); // or return custom error page
            }

            return RedirectToPage("/Dashboard/TrainerDashboard");
        }

        //private bool WorkoutSessionExists(int id)
        //{
        //    return _context.WorkoutSessions.Any(e => e.SessionId == id);
        //}
    }
}
