using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace GymMaster_RazorPages.Pages.BlogPost
{
    public class EditModel : PageModel
    {
        private readonly MSSQLServer.EntitiesModels.GymManagementContext _context;

        public EditModel(MSSQLServer.EntitiesModels.GymManagementContext context)
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

            var blogpost =  await _context.BlogPosts.FirstOrDefaultAsync(m => m.PostId == id);
            if (blogpost == null)
            {
                return NotFound();
            }
            BlogPost = blogpost;
           ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BlogPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(BlogPost.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.PostId == id);
        }
    }
}
