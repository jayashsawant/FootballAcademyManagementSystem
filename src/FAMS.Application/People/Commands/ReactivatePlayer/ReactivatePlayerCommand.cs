using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.ReactivatePlayer;

public sealed record ReactivatePlayerCommand(Guid PlayerId) : IRequest<Result>;