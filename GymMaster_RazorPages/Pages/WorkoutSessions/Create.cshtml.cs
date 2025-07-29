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

namespace GymMaster_RazorPages.Pages.WorkoutSessions
{
    [Authorize(Roles = "Trainer")]

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
        //public async Task<IActionResult> OnGet()
        //{
        //    MemberList = new SelectList(await _userService.GetAllAsync(), "UserId", "Email");
        //    PlanList = new SelectList(await _workoutPlanService.GetAllAsync(), "PlanId", "ExerciseName");
        //    return Page();
        //}

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

        public async Task<IActionResult> OnGet(int? memberId)
        {
            // Initialize empty lists
            List<User> membersList = new();
            IEnumerable<WorkoutPlan> plans = new List<WorkoutPlan>();

            if (memberId.HasValue)
            {
                // Get single member when memberId is provided
                var member = await _userService.GetByIdAsync(memberId.Value);
                if (member != null)
                {
                    membersList.Add(member);
                    WorkoutSession = new WorkoutSession
                    {
                        MemberId = memberId.Value,
                        CompletedAt = DateTime.Now
                    };

                    // Get plans specific to this member
                    plans = await _workoutPlanService.GetByMemberIdAsync(memberId.Value);
                }
            }
            else
            {
                // Get all members and plans when no memberId
                membersList = (await _userService.GetAllAsync()).ToList();
                plans = await _workoutPlanService.GetAllAsync();
            }

            MemberList = new SelectList(membersList, "UserId", "Email");
            PlanList = new SelectList(plans, "PlanId", "ExerciseName");

            return Page();
        }
    }


}
