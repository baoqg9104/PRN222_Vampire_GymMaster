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
    public class IndexModel : PageModel
    {
        private readonly IUserMembershipService _userMembershipService;
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly IUserService _userService;

        public IndexModel(IUserMembershipService userMembershipService, IMembershipPlanService membershipPlanService, IUserService userService)
        {
            _userMembershipService = userMembershipService;
            _membershipPlanService = membershipPlanService;
            _userService = userService;

        }
        [BindProperty(SupportsGet = true)]
        public string? searchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? date { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalPages { get; set; }


        public IList<UserMembership> UserMembership { get;set; } = default!;
        public IEnumerable<MSSQLServer.EntitiesModels.MembershipPlan> MembershipPlan { get; set; } = new List<MSSQLServer.EntitiesModels.MembershipPlan>();
        public IEnumerable<User> Users { get; set; } = new List<User>();

        public async Task OnGetAsync()
        {
            var result = await _userMembershipService.GetListAsync(searchTerm, date, PageIndex, 6);
            UserMembership = result.UserMembership;
            TotalPages = result.TotalPages;
            PageIndex = result.PageIndex;

            MembershipPlan = await _membershipPlanService.GetAllAsync();
            Users = await _userService.GetAllAsync();

        }
    }
}
