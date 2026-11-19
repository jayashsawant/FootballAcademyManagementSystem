using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.DeactivatePlayer;

public sealed record DeactivatePlayerCommand(Guid PlayerId) : IRequest<Result>;