using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.PublishNewsPost;

public sealed record PublishNewsPostCommand(Guid PostId) : IRequest<Result>;