using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetTeams;

public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, Result<IReadOnlyList<TeamDto>>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;

    public GetTeamsQueryHandler(
        ITeamRepository teamRepository,
        IBranchRepository branchRepository,
        ISeasonRepository seasonRepository,
        IAgeGroupRepository ageGroupRepository)
    {
        _teamRepository = teamRepository;
        _branchRepository = branchRepository;
        _seasonRepository = seasonRepository;
        _ageGroupRepository = ageGroupRepository;
    }

    public async Task<Result<IReadOnlyList<TeamDto>>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetAllAsync(
            request.BranchId, request.SeasonId, request.AgeGroupId, request.IncludeInactive, cancellationToken);

        var dtos = new List<TeamDto>();

        foreach (var team in teams)
        {
            var branch = await _branchRepository.GetByIdAsync(team.BranchId, cancellationToken);
            var season = await _seasonRepository.GetByIdAsync(team.SeasonId, cancellationToken);
            var ageGroup = await _ageGroupRepository.GetByIdAsync(team.AgeGroupId, cancellationToken);

            dtos.Add(new TeamDto(
                team.Id, team.BranchId, branch?.Name ?? "Unknown",
                team.SeasonId, season?.Name ?? "Unknown",
                team.AgeGroupId, ageGroup?.Name ?? "Unknown",
                team.Name, team.IsActive));
        }

        return dtos;
    }
}