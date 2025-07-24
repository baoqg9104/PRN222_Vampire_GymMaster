using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    public class CreateModel : PageModel
    {
        private readonly ITrainerAssignmentService _trainerAssignmentService;
        private readonly IUserService _userService;
        private readonly IUserMembershipService _membershipService;

        public CreateModel(
            ITrainerAssignmentService trainerAssignmentService,
            IUserService userService,
            IUserMembershipService membershipService)
        {
            _trainerAssignmentService = trainerAssignmentService;
            _userService = userService;
            _membershipService = membershipService;
        }

        [BindProperty]
        public TrainerAssignment TrainerAssignment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var members = await _userService.GetAllAsync();
            var trainers = await _userService.GetAllAsync();
            var memberships = await _membershipService.GetAllAsync();

            ViewData["MemberId"] = new SelectList(members, "UserId", "Email");
            ViewData["TrainerId"] = new SelectList(trainers, "UserId", "Email");
            ViewData["MembershipId"] = new SelectList(memberships, "MembershipId", "MembershipId");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var members = await _userService.GetAllAsync();
                var trainers = await _userService.GetAllAsync();
                var memberships = await _membershipService.GetAllAsync();

                ViewData["MemberId"] = new SelectList(members, "UserId", "Email", TrainerAssignment.MemberId);
                ViewData["TrainerId"] = new SelectList(trainers, "UserId", "Email", TrainerAssignment.TrainerId);
                ViewData["MembershipId"] = new SelectList(memberships, "MembershipId", "MembershipId", TrainerAssignment.MembershipId);

                return Page();
            }

            await _trainerAssignmentService.AddAsync(TrainerAssignment);

            return RedirectToPage("./Index");
        }
    }
}
