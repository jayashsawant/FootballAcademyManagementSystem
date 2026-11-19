using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateNewsPost;

public class UpdateNewsPostCommandHandler : IRequestHandler<UpdateNewsPostCommand, Result>
{
    private readonly INewsPostRepository _newsPostRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNewsPostCommandHandler(INewsPostRepository newsPostRepository, IUnitOfWork unitOfWork)
    {
        _newsPostRepository = newsPostRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateNewsPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _newsPostRepository.GetByIdAsync(request.PostId, cancellationToken);

        if (post is null)
        {
            return Result.Failure(Error.NotFound("NewsPost.NotFound", "The specified post could not be found."));
        }

        post.UpdateContent(request.Title, request.Body, request.CoverImageUrl);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}