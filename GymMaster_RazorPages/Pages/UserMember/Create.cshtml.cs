using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.UserMember
{
    public class CreateModel : PageModel
    {
        private readonly IUserMembershipService _userMembershipService;
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly IUserService _userService;

        public CreateModel(IUserMembershipService userMembershipService, IMembershipPlanService membershipPlanService, IUserService userService)
        {
            _userMembershipService = userMembershipService;
            _membershipPlanService = membershipPlanService;
            _userService = userService;

        }
        public SelectList UserList { get; set; } = default!;
        public SelectList PlanList { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            UserMembership = new UserMembership
            {
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
                IsActive = true,
                AutoRenew = false
            };
            UserList = new SelectList(await _userService.GetAllAsync(), "UserId", "Email");
            PlanList = new SelectList(await _membershipPlanService.GetAllAsync(), "PlanId", "Name");
            return Page();
        }

        [BindProperty]
        public UserMembership UserMembership { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}


            await _userMembershipService.AddAsync(UserMembership);


            return RedirectToPage("./Index");
        }
    }
}
