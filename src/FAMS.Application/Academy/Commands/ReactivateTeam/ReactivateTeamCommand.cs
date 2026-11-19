using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateTeam;

public sealed record ReactivateTeamCommand(Guid TeamId) : IRequest<Result>;