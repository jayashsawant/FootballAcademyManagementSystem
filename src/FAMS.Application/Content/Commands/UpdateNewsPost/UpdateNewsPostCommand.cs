using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateNewsPost;

public sealed record UpdateNewsPostCommand(Guid PostId, string Title, string Body, string? CoverImageUrl) : IRequest<Result>;