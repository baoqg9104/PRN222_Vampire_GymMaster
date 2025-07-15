using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using MSSQLServer.EntitiesModels;

namespace Services.Services;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _blogPostRepository;

    public BlogPostService(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        return await _blogPostRepository.AddAsync(blogPost);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _blogPostRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _blogPostRepository.GetAllAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetByAuthorIdAsync(int authorId)
    {
        return await _blogPostRepository.GetByAuthorIdAsync(authorId);
    }

    public async Task<BlogPost?> GetByIdAsync(int id)
    {
        return await _blogPostRepository.GetByIdAsync(id);
    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        return await _blogPostRepository.UpdateAsync(blogPost);
    }
}
