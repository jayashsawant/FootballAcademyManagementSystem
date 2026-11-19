using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateTeam;

public sealed record DeactivateTeamCommand(Guid TeamId) : IRequest<Result>;