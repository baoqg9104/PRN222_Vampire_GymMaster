using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace GymMaster_RazorPages.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly MSSQLServer.EntitiesModels.GymManagementContext _context;

        public IndexModel(MSSQLServer.EntitiesModels.GymManagementContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
