using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.UserMember
{
    public class EditModel : PageModel
    {
        private readonly IUserMembershipService _userMembershipService;
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly IUserService _userService;

        public EditModel(IUserMembershipService userMembershipService, IMembershipPlanService membershipPlanService, IUserService userService)
        {
            _userMembershipService = userMembershipService;
            _membershipPlanService = membershipPlanService;
            _userService = userService;

        }

        [BindProperty]
        public UserMembership UserMembership { get; set; } = default!;

        public SelectList UserList { get; set; } = default!;
        public SelectList PlanList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usermembership = await _userMembershipService.GetByIdAsync(id.Value);
            if (usermembership == null)
            {
                return NotFound();
            }
            UserMembership = usermembership;
            UserList = new SelectList(await _userService.GetAllAsync(), "UserId", "Email");
            PlanList = new SelectList(await _membershipPlanService.GetAllAsync(), "PlanId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(UserMembership).State = EntityState.Modified;
            try
            {
                await _userMembershipService.UpdateAsync(UserMembership);
            }
            catch (Exception)
            {
                return NotFound(); // or return custom error page
            }

            return RedirectToPage("./Index");
        }

        //private bool UserMembershipExists(int id)
        //{
        //    return _context.UserMemberships.Any(e => e.MembershipId == id);
        //}
    }
}
