using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System.Security.Claims;

namespace GymMaster_RazorPages.Pages.Dashboard
{
    public class MemberDashboardModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserMembershipService _membershipService;
        private readonly ITrainerAssignmentService _trainerService;

        [BindProperty]
        public User CurrentUser { get; set; }
        public UserMembership CurrentMembership { get; set; }

        // Changed from single trainer to list of active assignments
        public List<TrainerAssignment> ActiveTrainerAssignments { get; set; } = new List<TrainerAssignment>();

        public MemberDashboardModel(
            IUserService userService,
            IUserMembershipService membershipService,
            ITrainerAssignmentService trainerService)
        {
            _userService = userService;
            _membershipService = membershipService;
            _trainerService = trainerService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CurrentUser = await _userService.GetByIdAsync(userId);

            if (CurrentUser == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Load current membership
            CurrentMembership = await _membershipService.GetCurrentMembershipAsync(userId);

            // Load all active trainer assignments
            ActiveTrainerAssignments = await _trainerService.GetActiveTrainerAssignmentsByMemberIdAsync(userId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload data if validation fails
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                CurrentMembership = await _membershipService.GetCurrentMembershipAsync(userId);
                ActiveTrainerAssignments = await _trainerService.GetActiveTrainerAssignmentsByMemberIdAsync(userId);
                return Page();
            }

            try
            {
                await _userService.UpdateAsync(CurrentUser);
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi cập nhật: {ex.Message}");

                // Reload data if update fails
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                CurrentMembership = await _membershipService.GetCurrentMembershipAsync(userId);
                ActiveTrainerAssignments = await _trainerService.GetActiveTrainerAssignmentsByMemberIdAsync(userId);
                return Page();
            }
        }
    }
}