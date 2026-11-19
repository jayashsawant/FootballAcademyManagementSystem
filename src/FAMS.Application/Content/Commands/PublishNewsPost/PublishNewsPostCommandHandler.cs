using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.PublishNewsPost;

public class PublishNewsPostCommandHandler : IRequestHandler<PublishNewsPostCommand, Result>
{
    private readonly INewsPostRepository _newsPostRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishNewsPostCommandHandler(INewsPostRepository newsPostRepository, IUnitOfWork unitOfWork)
    {
        _newsPostRepository = newsPostRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PublishNewsPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _newsPostRepository.GetByIdAsync(request.PostId, cancellationToken);

        if (post is null)
        {
            return Result.Failure(Error.NotFound("NewsPost.NotFound", "The specified post could not be found."));
        }

        post.Publish();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}