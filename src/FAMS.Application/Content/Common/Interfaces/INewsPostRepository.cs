using FAMS.Domain.Content;

namespace FAMS.Application.Content.Common.Interfaces;

public interface INewsPostRepository
{
    Task<NewsPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NewsPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludePostId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<NewsPost> Posts, int TotalCount)> SearchAsync(
        NewsPostStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(NewsPost post, CancellationToken cancellationToken = default);
    Task RemoveAsync(NewsPost post, CancellationToken cancellationToken = default);
}