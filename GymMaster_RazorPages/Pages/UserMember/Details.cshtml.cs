using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]

    public class DetailsModel : PageModel
    {
        private readonly IUserMembershipService _userMembershipService;

        public DetailsModel(IUserMembershipService userMembershipService)
        {
            _userMembershipService = userMembershipService;

        }

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
    }
}
