using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.AddGalleryImage;

public sealed record AddGalleryImageCommand(string ImageUrl, string? Caption, int SortOrder) : IRequest<Result<GalleryImageDto>>;