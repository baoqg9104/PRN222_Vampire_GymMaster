using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.MembershipPlan
{
    // Authorize 
    //[Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IMembershipPlanService membershipPlanService, ILogger<DeleteModel> logger)
        {
            _membershipPlanService = membershipPlanService;
            _logger = logger;
        }

        [BindProperty]
        public MSSQLServer.EntitiesModels.MembershipPlan MembershipPlan { get; set; } = default!;

        public bool HasActiveMembers { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }

        // Replace all instances of comparing UserMembership.EndDate (DateOnly) with DateTime.Now
        // with a comparison to DateOnly.FromDateTime(DateTime.Now)

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _membershipPlanService.GetByIdAsync(id.Value);
            if (membershipPlan == null)
            {
                _logger.LogWarning("MembershipPlan with ID {PlanId} not found during delete attempt", id);
                return NotFound();
            }

            MembershipPlan = membershipPlan;

            // Check for active members by comparing EndDate with today's date
            HasActiveMembers = membershipPlan.UserMemberships != null &&
                membershipPlan.UserMemberships.Any(um => um.EndDate > DateOnly.FromDateTime(DateTime.Now));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _membershipPlanService.GetByIdAsync(id.Value);
            if (membershipPlan == null)
            {
                _logger.LogWarning("MembershipPlan with ID {PlanId} not found during delete confirmation", id);
                return NotFound();
            }

            var hasActiveUsers = membershipPlan.UserMemberships != null &&
                membershipPlan.UserMemberships.Any(um => um.EndDate > DateOnly.FromDateTime(DateTime.Now));

            if (hasActiveUsers)
            {
                StatusMessage = "Error: Cannot delete this plan because it has active members. Deactivate the plan instead.";
                return RedirectToPage("./Index");
            }

            try
            {
                if (Request.Form["deleteType"] == "soft")
                {
                    // Soft delete: update the plan to inactive
                    membershipPlan.IsActive = false;
                    await _membershipPlanService.UpdateAsync(membershipPlan);
                    _logger.LogInformation("MembershipPlan with ID {PlanId} was soft-deleted by {User}", id, User.Identity?.Name);
                    StatusMessage = "Membership plan has been deactivated.";
                }
                else
                {
                    // Permanent deletion
                    bool result = await _membershipPlanService.DeleteAsync(id.Value);
                    if (result)
                    {
                        _logger.LogInformation("MembershipPlan with ID {PlanId} was permanently deleted by {User}", id, User.Identity?.Name);
                        StatusMessage = "Membership plan has been permanently deleted.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete MembershipPlan with ID {PlanId}", id);
                StatusMessage = "Error: Failed to delete the membership plan. It may have associated records.";
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
