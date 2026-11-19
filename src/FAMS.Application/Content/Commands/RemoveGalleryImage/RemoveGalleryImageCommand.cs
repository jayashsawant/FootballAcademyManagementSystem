using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.RemoveGalleryImage;

public sealed record RemoveGalleryImageCommand(Guid ImageId) : IRequest<Result>;