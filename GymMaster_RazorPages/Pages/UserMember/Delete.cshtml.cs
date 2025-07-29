using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.UserMember
{
    public class DeleteModel : PageModel
    {
        private readonly IUserMembershipService _userMembershipService;

        public DeleteModel(IUserMembershipService userMembershipService)
        {
            _userMembershipService = userMembershipService;

        }

        [BindProperty]
        public UserMembership UserMembership { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usermembership = await _userMembershipService.GetByIdAsync(id.Value);

            if (usermembership is not null)
            {
                UserMembership = usermembership;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usermembership = await _userMembershipService.GetByIdAsync(id.Value);
            if (usermembership != null)
            {
                UserMembership = usermembership;
                await _userMembershipService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
