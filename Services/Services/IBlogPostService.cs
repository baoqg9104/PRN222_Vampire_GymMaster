using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSSQLServer.EntitiesModels;

namespace Services.Services;

public interface IBlogPostService
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<BlogPost?> GetByIdAsync(int id);
    Task<IEnumerable<BlogPost>> GetByAuthorIdAsync(int authorId);
    Task<BlogPost> AddAsync(BlogPost blogPost);
    Task<BlogPost> UpdateAsync(BlogPost blogPost);
    Task<bool> DeleteAsync(int id);
    Task<int> GetTotalBlogPostsCountAsync();
}
