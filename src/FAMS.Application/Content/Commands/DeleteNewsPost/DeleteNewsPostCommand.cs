using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.DeleteNewsPost;

public sealed record DeleteNewsPostCommand(Guid PostId) : IRequest<Result>;