using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateTeam;

public sealed record UpdateTeamCommand(Guid TeamId, string Name) : IRequest<Result>;