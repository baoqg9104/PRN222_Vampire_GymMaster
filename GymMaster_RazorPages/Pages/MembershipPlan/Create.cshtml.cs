using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.MembershipPlan
{
    // [Authorize(Roles = "Admin")] // Uncomment if needed
    public class CreateModel : PageModel
    {
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IMembershipPlanService membershipPlanService, ILogger<CreateModel> logger)
        {
            _membershipPlanService = membershipPlanService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Initialize with default values
            MembershipPlan = new MSSQLServer.EntitiesModels.MembershipPlan
            {
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            return Page();
        }

        [BindProperty]
        public MSSQLServer.EntitiesModels.MembershipPlan MembershipPlan { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; }

        // Custom validation to ensure price is reasonable
        public bool ValidatePrice()
        {
            if (MembershipPlan.Price < 0)
            {
                ModelState.AddModelError("MembershipPlan.Price", "Price cannot be negative.");
                return false;
            }
            if (MembershipPlan.Price > 10000)
            {
                ModelState.AddModelError("MembershipPlan.Price", "Price seems unreasonably high. Please verify.");
                return false;
            }
            return true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ValidatePrice())
            {
                return Page();
            }

            // Sanitize inputs
            if (!string.IsNullOrWhiteSpace(MembershipPlan.Name))
            {
                MembershipPlan.Name = MembershipPlan.Name.Trim();
            }
            if (!string.IsNullOrWhiteSpace(MembershipPlan.Description))
            {
                MembershipPlan.Description = MembershipPlan.Description.Trim();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var plan = await _membershipPlanService.AddAsync(MembershipPlan);
                _logger.LogInformation("New MembershipPlan created: {PlanName} (ID: {PlanId}) by {UserName}", 
                    plan.Name, plan.PlanId, User.Identity?.Name);
                StatusMessage = "Membership plan created successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new MembershipPlan");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the membership plan. Please try again.");
                return Page();
            }
        }
    }
}
