using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.BlogPost
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public CreateModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MSSQLServer.EntitiesModels.BlogPost BlogPost { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("BlogPost.Author");
            ModelState.Remove("BlogPost.Slug");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set the AuthorId to the current user's ID
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Slug can be generated based on the title or other criteria
            BlogPost.Slug = BlogPost.Title.ToLower().Replace(" ", "-").Replace("'", "").Replace("\"", "");
            BlogPost.AuthorId = userId;
            BlogPost.CreatedAt = BlogPost.PublishedAt = DateTime.Now;
            BlogPost.IsPublished = true; // Set default value for IsActive

            await _blogPostService.AddAsync(BlogPost);

            return RedirectToPage("./Index");
        }
    }
}
