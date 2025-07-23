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
        public User CurrentTrainer { get; set; }

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

            CurrentMembership = await _membershipService.GetCurrentMembershipAsync(userId);
            CurrentTrainer = await _trainerService.GetCurrentTrainerAsync(userId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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
                return Page();
            }
        }
    }
}
