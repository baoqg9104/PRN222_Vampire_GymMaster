using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    public class IndexModel : PageModel
    {
        private readonly MSSQLServer.EntitiesModels.GymManagementContext _context;

        public IndexModel(MSSQLServer.EntitiesModels.GymManagementContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public IList<TrainerAssignment> TrainerAssignment { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var query = _context.TrainerAssignments
                .Include(t => t.Member)
                .Include(t => t.Membership)
                .Include(t => t.Trainer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim().ToLower();
                query = query.Where(t =>
                    t.Member.Email.ToLower().Contains(term) ||
                    t.Trainer.Email.ToLower().Contains(term) ||
                    t.Goals != null && t.Goals.ToLower().Contains(term) ||
                    t.Membership.MembershipId.ToString().Contains(term)
                );
            }

            TrainerAssignment = await query.ToListAsync();
        }
    }
}
