using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Services.Services;

namespace GymMaster_RazorPages.Pages.Dashboard
{
    // Authorize 
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly IBlogPostService _blogPostService;
        private readonly ITrainerAssignmentService _trainerAssignmentService;
        private readonly ILogger<AdminDashboardModel> _logger;

        public AdminDashboardModel(
            IUserService userService,
            IMembershipPlanService membershipPlanService,
            IBlogPostService blogPostService,
            ITrainerAssignmentService trainerAssignmentService,
            ILogger<AdminDashboardModel> logger)
        {
            _userService = userService;
            _membershipPlanService = membershipPlanService;
            _blogPostService = blogPostService;
            _trainerAssignmentService = trainerAssignmentService;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // Statistics properties needed by the view
        public int TotalUsers { get; set; }
        public int TrainerCount { get; set; }
        public int MemberCount { get; set; }
        public int ActiveMemberships { get; set; }
        public int TotalMembershipPlans { get; set; }
        public int TotalBlogPosts { get; set; }
        public int NewUsersThisMonth { get; set; }

        // TrainerAssignment statistics
        public int TotalTrainerAssignments { get; set; }
        public int ActiveTrainerAssignments { get; set; }
        public int PendingTrainerAssignments { get; set; }
        public int CompletedTrainerAssignments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Calculate current date ranges for statistics
                var now = DateTime.Now;
                var firstDayOfMonth = new DateOnly(now.Year, now.Month, 1);

                // User statistics
                TotalUsers = await _userService.GetTotalUsersCountAsync();
                TrainerCount = await _userService.GetTrainerCountAsync();
                MemberCount = await _userService.GetMemberCountAsync();
                NewUsersThisMonth = await _userService.GetNewUsersThisMonthAsync();

                // Membership statistics
                ActiveMemberships = await _membershipPlanService.GetActiveMembershipsCountAsync();
                TotalMembershipPlans = await _membershipPlanService.GetTotalMembershipPlansCountAsync();

                // Blog statistics - Get all blog posts count
                TotalBlogPosts = await _blogPostService.GetTotalBlogPostsCountAsync();

                // TrainerAssignment statistics
                //TotalTrainerAssignments = await _trainerAssignmentService.GetTotalTrainerAssignmentsCountAsync();
                //ActiveTrainerAssignments = await _trainerAssignmentService.GetActiveTrainerAssignmentsCountAsync();
                //PendingTrainerAssignments = await _trainerAssignmentService.GetPendingTrainerAssignmentsCountAsync();
                //CompletedTrainerAssignments = await _trainerAssignmentService.GetCompletedTrainerAssignmentsCountAsync();

                _logger.LogInformation("Admin dashboard loaded successfully for {User}", User.Identity?.Name);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard data");
                // Initialize with default values in case of error
                TotalUsers = 0;
                TrainerCount = 0;
                MemberCount = 0;
                ActiveMemberships = 0;
                TotalMembershipPlans = 0;
                TotalBlogPosts = 0;
                NewUsersThisMonth = 0;
                TotalTrainerAssignments = 0;
                ActiveTrainerAssignments = 0;
                PendingTrainerAssignments = 0;
                CompletedTrainerAssignments = 0;

                StatusMessage = "An error occurred while loading some dashboard data. Statistics may not be accurate.";
                return Page();
            }
        }
    }
}