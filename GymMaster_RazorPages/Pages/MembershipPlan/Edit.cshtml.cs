using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSSQLServer.EntitiesModels;

namespace GymMaster_RazorPages.Pages.MembershipPlan
{
    // [Authorize(Roles = "Admin")] // Uncomment if authorization is needed
    public class EditModel : PageModel
    {
        private readonly GymManagementContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(GymManagementContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public MSSQLServer.EntitiesModels.MembershipPlan MembershipPlan { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipplan = await _context.MembershipPlans.FirstOrDefaultAsync(m => m.PlanId == id);
            
            if (membershipplan == null)
            {
                _logger.LogWarning("MembershipPlan with ID {PlanId} was not found", id);
                return NotFound();
            }
            
            MembershipPlan = membershipplan;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the original entity to preserve created date
            var originalPlan = await _context.MembershipPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PlanId == MembershipPlan.PlanId);
            
            if (originalPlan == null)
            {
                return NotFound();
            }

            // Preserve the original created date
            MembershipPlan.CreatedAt = originalPlan.CreatedAt;
            
            // Mark entity as modified
            _context.Attach(MembershipPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("MembershipPlan with ID {PlanId} was successfully updated by {User}", 
                    MembershipPlan.PlanId, User.Identity?.Name);
                
                StatusMessage = "Membership plan updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipPlanExists(MembershipPlan.PlanId))
                {
                    _logger.LogWarning("Concurrency exception: MembershipPlan with ID {PlanId} was not found", MembershipPlan.PlanId);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Failed to update MembershipPlan with ID {PlanId}", MembershipPlan.PlanId);
                    ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MembershipPlanExists(int id)
        {
            return _context.MembershipPlans.Any(e => e.PlanId == id);
        }
    }
}
