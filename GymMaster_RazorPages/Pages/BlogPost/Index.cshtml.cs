using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.BlogPost
{
    public class IndexModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;
        //private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public IList<MSSQLServer.EntitiesModels.BlogPost> BlogPost { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; } = "all"; // Default to show all posts

        public async Task OnGetAsync()
        {
            var allPosts = await _blogPostService.GetAllAsync();

            if (Filter == "mine" && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                BlogPost = allPosts
                    .Where(p => p.AuthorId == int.Parse(userId))
                    .ToList();
            }
            else
            {
                // Show all posts
                BlogPost = allPosts.ToList();
            }
        }
    }
}
