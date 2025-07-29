using Microsoft.AspNetCore.Authorization;
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

namespace GymMaster_RazorPages.Pages.BlogPost
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public EditModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [BindProperty]
        public MSSQLServer.EntitiesModels.BlogPost BlogPost { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogpost =  await _blogPostService.GetByIdAsync((int)id);
            if (blogpost == null)
            {
                return NotFound();
            }
            BlogPost = blogpost;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //ModelState.Remove("BlogPost.PostId");
            //ModelState.Remove("BlogPost.AuthorId");
            ModelState.Remove("BlogPost.Author");
            ModelState.Remove("BlogPost.Slug");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            BlogPost.Slug = BlogPost.Title.ToLower().Replace(" ", "-").Replace("'", "").Replace("\"", "");

            if (BlogPost.IsPublished == true)
            {
                BlogPost.PublishedAt = DateTime.Now;
            }

            await _blogPostService.UpdateAsync(BlogPost);

            return RedirectToPage("./Index");
        }
    }
}
