using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MSSQLServer.EntitiesModels;

namespace Infrastructure.Implements;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly GymManagementContext _context;

    public BlogPostRepository(GymManagementContext context)
    {
        _context = context;
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        if (blogPost == null)
        {
            throw new ArgumentNullException(nameof(blogPost), "Blog post cannot be null");
        }
        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var blogPost = await GetByIdAsync(id);
        if (blogPost == null)
        {
            return false;
        }
        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _context.BlogPosts
            .Include(bp => bp.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetByAuthorIdAsync(int authorId)
    {
        return await _context.BlogPosts
            .Where(bp => bp.AuthorId == authorId)
            .Include(bp => bp.Author)
            .ToListAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(int id)
    {
        return await _context.BlogPosts
            .Include(bp => bp.Author)
            .FirstOrDefaultAsync(bp => bp.PostId == id);
    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        if (blogPost == null)
        {
            throw new ArgumentNullException(nameof(blogPost), "Blog post cannot be null");
        }
        _context.BlogPosts.Update(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
    }
}
