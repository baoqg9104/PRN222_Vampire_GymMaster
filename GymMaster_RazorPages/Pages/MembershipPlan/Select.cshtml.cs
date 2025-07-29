using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using System.Security.Claims;

namespace GymMaster_RazorPages.Pages.MembershipPlan;

[Authorize(Roles = "Member,Admin")]
public class SelectModel : PageModel
{

    private readonly IMembershipPlanService _membershipPlanService;
    private readonly IUserMembershipService _userMembershipService;

    public SelectModel(IMembershipPlanService membershipPlanService, IUserMembershipService userMembershipService)
    {
        _membershipPlanService = membershipPlanService;
        _userMembershipService = userMembershipService;
    }

    [BindProperty]
    public MSSQLServer.EntitiesModels.MembershipPlan MembershipPlan { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? planId)
    {
        if (planId == null)
        {
            return NotFound();
        }

        var membershipPlan = await _membershipPlanService.GetByIdAsync((int)planId);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        MembershipPlan = membershipPlan;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int planId, bool autoRenew)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var membershipPlan = await _membershipPlanService.GetByIdAsync(planId);

        if (membershipPlan == null)
        {
            TempData["ErrorMessage"] = "Selected membership plan does not exist.";
            return Page();
        }

        var duration = membershipPlan.DurationDays;
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(duration);

        if (endDate <= startDate)
        {
            TempData["ErrorMessage"] = duration;
            return Page();
        }

        var userMembership = new MSSQLServer.EntitiesModels.UserMembership
        {
            UserId = userId,
            PlanId = planId,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            AutoRenew = autoRenew
        };

        await _userMembershipService.AddAsync(userMembership);
        //TempData["StatusMessage"] = "Membership plan selected successfully!";
        return RedirectToPage("/Dashboard/MemberDashboard");
    }
}
