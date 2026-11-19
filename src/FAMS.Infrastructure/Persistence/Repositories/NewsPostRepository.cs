using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class NewsPostRepository : INewsPostRepository
{
    private readonly ApplicationDbContext _context;

    public NewsPostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<NewsPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.NewsPosts.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public Task<NewsPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return _context.NewsPosts.FirstOrDefaultAsync(n => n.Slug == slug, cancellationToken);
    }

    public Task<bool> SlugExistsAsync(string slug, Guid? excludePostId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.NewsPosts.Where(n => n.Slug == slug);

        if (excludePostId.HasValue)
        {
            query = query.Where(n => n.Id != excludePostId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<NewsPost> Posts, int TotalCount)> SearchAsync(
        NewsPostStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.NewsPosts.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(n => n.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var posts = await query
            .OrderByDescending(n => n.PublishedAt ?? n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (posts, totalCount);
    }

    public async Task AddAsync(NewsPost post, CancellationToken cancellationToken = default)
    {
        await _context.NewsPosts.AddAsync(post, cancellationToken);
    }

    public Task RemoveAsync(NewsPost post, CancellationToken cancellationToken = default)
    {
        _context.NewsPosts.Remove(post);
        return Task.CompletedTask;
    }
}