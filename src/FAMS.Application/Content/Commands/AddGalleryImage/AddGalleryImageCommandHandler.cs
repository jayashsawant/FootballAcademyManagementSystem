using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.AddGalleryImage;

public class AddGalleryImageCommandHandler : IRequestHandler<AddGalleryImageCommand, Result<GalleryImageDto>>
{
    private readonly IGalleryImageRepository _galleryImageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddGalleryImageCommandHandler(IGalleryImageRepository galleryImageRepository, IUnitOfWork unitOfWork)
    {
        _galleryImageRepository = galleryImageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GalleryImageDto>> Handle(AddGalleryImageCommand request, CancellationToken cancellationToken)
    {
        var image = GalleryImage.Create(request.ImageUrl, request.Caption, request.SortOrder);

        await _galleryImageRepository.AddAsync(image, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new GalleryImageDto(image.Id, image.ImageUrl, image.Caption, image.SortOrder);
    }
}