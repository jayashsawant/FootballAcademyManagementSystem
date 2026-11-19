using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.CreateNewsPost;

public class CreateNewsPostCommandHandler : IRequestHandler<CreateNewsPostCommand, Result<NewsPostDto>>
{
    private readonly INewsPostRepository _newsPostRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNewsPostCommandHandler(INewsPostRepository newsPostRepository, IUnitOfWork unitOfWork)
    {
        _newsPostRepository = newsPostRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<NewsPostDto>> Handle(CreateNewsPostCommand request, CancellationToken cancellationToken)
    {
        var slugExists = await _newsPostRepository.SlugExistsAsync(request.Slug, cancellationToken: cancellationToken);

        if (slugExists)
        {
            return Result<NewsPostDto>.Failure(Error.Conflict("NewsPost.SlugInUse", "A post with this URL slug already exists."));
        }

        var post = NewsPost.Create(request.Title, request.Slug, request.Body, request.CoverImageUrl);

        await _newsPostRepository.AddAsync(post, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new NewsPostDto(post.Id, post.Title, post.Slug, post.Body, post.CoverImageUrl, post.Status.ToString(), post.PublishedAt, post.CreatedAt);
    }
}