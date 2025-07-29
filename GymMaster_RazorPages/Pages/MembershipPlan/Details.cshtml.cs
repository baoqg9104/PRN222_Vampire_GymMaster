using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.MembershipPlan
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IMembershipPlanService membershipPlanService, ILogger<DetailsModel> logger)
        {
            _membershipPlanService = membershipPlanService;
            _logger = logger;
        }

        public MSSQLServer.EntitiesModels.MembershipPlan MembershipPlan { get; set; } = default!;

        // Statistics properties
        public int ActiveMembersCount { get; set; }
        public int TotalMembersEver { get; set; }
        public decimal TotalRevenue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("MembershipPlan Details accessed with null ID");
                return NotFound();
            }

            var membershipPlan = await _membershipPlanService.GetByIdAsync(id.Value);
            if (membershipPlan == null)
            {
                _logger.LogWarning("MembershipPlan with ID {PlanId} not found", id);
                return NotFound();
            }

            MembershipPlan = membershipPlan;

            // Calculate statistics if UserMemberships are available
            if (MembershipPlan.UserMemberships != null)
            {
                var now = DateTime.Now;
                // Compare EndDate (DateOnly) with current date in DateOnly format
                ActiveMembersCount = MembershipPlan.UserMemberships
                    .Count(um => um.EndDate > DateOnly.FromDateTime(now));

                TotalMembersEver = MembershipPlan.UserMemberships.Count;
                TotalRevenue = TotalMembersEver * MembershipPlan.Price;
            }

            _logger.LogInformation("MembershipPlan details viewed for plan ID {PlanId}", id);
            return Page();
        }

        // Helper method to format duration in a human-readable way
        public string FormatDuration(int days)
        {
            if (days < 30)
                return $"{days} days";
            else if (days < 365)
                return $"{days / 30} months" + (days % 30 > 0 ? $" and {days % 30} days" : "");
            else
                return $"{days / 365} years" + (days % 365 > 0 ? $" and {(days % 365) / 30} months" : "");
        }
    }
}
