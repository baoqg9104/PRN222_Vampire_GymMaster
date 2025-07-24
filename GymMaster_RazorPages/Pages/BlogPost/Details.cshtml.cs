using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.BlogPost
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public DetailsModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public MSSQLServer.EntitiesModels.BlogPost BlogPost { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogpost = await _blogPostService.GetByIdAsync((int)id);

            if (blogpost is not null)
            {
                BlogPost = blogpost;

                return Page();
            }

            return NotFound();
        }
    }
}
