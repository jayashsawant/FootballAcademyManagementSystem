using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Queries.GetNewsPostBySlug;

public class GetNewsPostBySlugQueryHandler : IRequestHandler<GetNewsPostBySlugQuery, Result<NewsPostDto>>
{
    private readonly INewsPostRepository _newsPostRepository;

    public GetNewsPostBySlugQueryHandler(INewsPostRepository newsPostRepository)
    {
        _newsPostRepository = newsPostRepository;
    }

    public async Task<Result<NewsPostDto>> Handle(GetNewsPostBySlugQuery request, CancellationToken cancellationToken)
    {
        var post = await _newsPostRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (post is null || post.Status != NewsPostStatus.Published)
        {
            return Result<NewsPostDto>.Failure(Error.NotFound("NewsPost.NotFound", "The specified post could not be found."));
        }

        return new NewsPostDto(post.Id, post.Title, post.Slug, post.Body, post.CoverImageUrl, post.Status.ToString(), post.PublishedAt, post.CreatedAt);
    }
}