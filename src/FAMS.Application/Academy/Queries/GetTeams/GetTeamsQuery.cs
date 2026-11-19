using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetTeams;

public sealed record GetTeamsQuery(
    Guid? BranchId,
    Guid? SeasonId,
    Guid? AgeGroupId,
    bool IncludeInactive = false) : IRequest<Result<IReadOnlyList<TeamDto>>>;