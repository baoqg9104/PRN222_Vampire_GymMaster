using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace GymMaster_RazorPages.Pages.BlogPost
{
    public class DeleteModel : PageModel
    {
        private readonly MSSQLServer.EntitiesModels.GymManagementContext _context;

        public DeleteModel(MSSQLServer.EntitiesModels.GymManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MSSQLServer.EntitiesModels.BlogPost BlogPost { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogpost = await _context.BlogPosts.FirstOrDefaultAsync(m => m.PostId == id);

            if (blogpost is not null)
            {
                BlogPost = blogpost;

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

            var blogpost = await _context.BlogPosts.FindAsync(id);
            if (blogpost != null)
            {
                BlogPost = blogpost;
                _context.BlogPosts.Remove(BlogPost);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
