using FAMS.Application.Common.Models;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetNewsPosts;

public class GetNewsPostsQueryHandler : IRequestHandler<GetNewsPostsQuery, Result<PagedResult<NewsPostDto>>>
{
    private readonly INewsPostRepository _newsPostRepository;

    public GetNewsPostsQueryHandler(INewsPostRepository newsPostRepository)
    {
        _newsPostRepository = newsPostRepository;
    }

    public async Task<Result<PagedResult<NewsPostDto>>> Handle(GetNewsPostsQuery request, CancellationToken cancellationToken)
    {
        var (posts, totalCount) = await _newsPostRepository.SearchAsync(request.Status, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = posts
            .Select(p => new NewsPostDto(p.Id, p.Title, p.Slug, p.Body, p.CoverImageUrl, p.Status.ToString(), p.PublishedAt, p.CreatedAt))
            .ToList();

        return new PagedResult<NewsPostDto>(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}