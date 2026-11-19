using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetGalleryImages;

public sealed record GetGalleryImagesQuery : IRequest<Result<IReadOnlyList<GalleryImageDto>>>;