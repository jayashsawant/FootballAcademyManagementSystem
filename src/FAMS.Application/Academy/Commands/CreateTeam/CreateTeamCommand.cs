using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateTeam;

public sealed record CreateTeamCommand(
    Guid BranchId,
    Guid SeasonId,
    Guid AgeGroupId,
    string Name) : IRequest<Result<TeamDto>>;