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

        // Changed to group assignments by membership plan
        public Dictionary<int, List<TrainerAssignment>> TrainersByMembership { get; set; }
            = new Dictionary<int, List<TrainerAssignment>>();

        public List<UserMembership> MemberMemberships { get; set; } = new List<UserMembership>();


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
            MemberMemberships = await _membershipService.GetMembershipsByUserIdAsync(userId);

            CurrentMembership = MemberMemberships.FirstOrDefault(m =>
        m.StartDate <= DateOnly.FromDateTime(DateTime.Today) &&
        m.EndDate >= DateOnly.FromDateTime(DateTime.Today));

            //CurrentMembership = await _membershipService.GetCurrentMembershipAsync(userId);

            // Load all active trainer assignments and group by membership
            var activeAssignments = await _trainerService.GetActiveTrainerAssignmentsByMemberIdAsync(userId);

            if (activeAssignments != null && activeAssignments.Any())
            {
                TrainersByMembership = activeAssignments
                    .GroupBy(a => a.MembershipId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList()
                    );
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload data if validation fails
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                CurrentMembership = await _membershipService.GetCurrentMembershipAsync(userId);
                var activeAssignments = await _trainerService.GetActiveTrainerAssignmentsByMemberIdAsync(userId);

                if (activeAssignments != null && activeAssignments.Any())
                {
                    TrainersByMembership = activeAssignments
                        .GroupBy(a => a.MembershipId)
                        .ToDictionary(
                            g => g.Key,
                            g => g.ToList()
                        );
                }

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
                var activeAssignments = await _trainerService.GetActiveTrainerAssignmentsByMemberIdAsync(userId);

                if (activeAssignments != null && activeAssignments.Any())
                {
                    TrainersByMembership = activeAssignments
                        .GroupBy(a => a.MembershipId)
                        .ToDictionary(
                            g => g.Key,
                            g => g.ToList()
                        );
                }

                return Page();
            }
        }
    }
}