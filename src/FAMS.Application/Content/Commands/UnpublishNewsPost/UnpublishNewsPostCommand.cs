using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.UnpublishNewsPost;

public sealed record UnpublishNewsPostCommand(Guid PostId) : IRequest<Result>;