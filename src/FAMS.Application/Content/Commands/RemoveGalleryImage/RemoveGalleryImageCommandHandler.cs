using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.RemoveGalleryImage;

public class RemoveGalleryImageCommandHandler : IRequestHandler<RemoveGalleryImageCommand, Result>
{
    private readonly IGalleryImageRepository _galleryImageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveGalleryImageCommandHandler(IGalleryImageRepository galleryImageRepository, IUnitOfWork unitOfWork)
    {
        _galleryImageRepository = galleryImageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveGalleryImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _galleryImageRepository.GetByIdAsync(request.ImageId, cancellationToken);

        if (image is null)
        {
            return Result.Failure(Error.NotFound("GalleryImage.NotFound", "The specified image could not be found."));
        }

        await _galleryImageRepository.RemoveAsync(image, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}