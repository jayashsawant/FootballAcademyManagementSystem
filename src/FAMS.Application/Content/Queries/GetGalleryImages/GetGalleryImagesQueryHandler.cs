using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetGalleryImages;

public class GetGalleryImagesQueryHandler : IRequestHandler<GetGalleryImagesQuery, Result<IReadOnlyList<GalleryImageDto>>>
{
    private readonly IGalleryImageRepository _galleryImageRepository;

    public GetGalleryImagesQueryHandler(IGalleryImageRepository galleryImageRepository)
    {
        _galleryImageRepository = galleryImageRepository;
    }

    public async Task<Result<IReadOnlyList<GalleryImageDto>>> Handle(GetGalleryImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _galleryImageRepository.GetAllAsync(cancellationToken);

        var dtos = images
            .OrderBy(i => i.SortOrder)
            .Select(i => new GalleryImageDto(i.Id, i.ImageUrl, i.Caption, i.SortOrder))
            .ToList();

        return dtos;
    }
}