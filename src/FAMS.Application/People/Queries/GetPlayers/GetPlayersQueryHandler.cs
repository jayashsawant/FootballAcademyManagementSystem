using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Models;
using FAMS.Application.People.Common.Dtos;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetPlayers;

public sealed class GetPlayersQueryHandler
    : IRequestHandler<GetPlayersQuery, Result<PagedResult<PlayerDto>>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IParentRepository _parentRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly ITeamRepository _teamRepository;

    public GetPlayersQueryHandler(
        IPlayerRepository playerRepository,
        IParentRepository parentRepository,
        IBranchRepository branchRepository,
        ISeasonRepository seasonRepository,
        IAgeGroupRepository ageGroupRepository,
        IBatchRepository batchRepository,
        ITeamRepository teamRepository)
    {
        _playerRepository = playerRepository;
        _parentRepository = parentRepository;
        _branchRepository = branchRepository;
        _seasonRepository = seasonRepository;
        _ageGroupRepository = ageGroupRepository;
        _batchRepository = batchRepository;
        _teamRepository = teamRepository;
    }

    public async Task<Result<PagedResult<PlayerDto>>> Handle(
        GetPlayersQuery request,
        CancellationToken cancellationToken)
    {
        var (players, totalCount) =
            await _playerRepository.SearchAsync(
                request.SearchTerm,
                request.BranchId,
                request.SeasonId,
                request.AgeGroupId,
                request.BatchId,
                request.TeamId,
                request.ParentId,
                request.Status,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

        var playerDtos = new List<PlayerDto>();

        foreach (var player in players)
        {
            var parent =
                await _parentRepository.GetByIdAsync(
                    player.ParentId,
                    cancellationToken);

            var enrollment = player.Enrollments
                .FirstOrDefault(e => e.IsCurrent);

            Guid? enrollmentId = null;

            Guid? seasonId = null;
            string? seasonName = null;

            Guid? branchId = null;
            string? branchName = null;

            Guid? ageGroupId = null;
            string? ageGroupName = null;

            Guid? batchId = null;
            string? batchName = null;

            Guid? teamId = null;
            string? teamName = null;

            if (enrollment is not null)
            {
                enrollmentId = enrollment.Id;

                seasonId = enrollment.SeasonId;
                branchId = enrollment.BranchId;
                ageGroupId = enrollment.AgeGroupId;
                batchId = enrollment.BatchId;
                teamId = enrollment.TeamId;

                var season =
                    await _seasonRepository.GetByIdAsync(
                        enrollment.SeasonId,
                        cancellationToken);

                var branch =
                    await _branchRepository.GetByIdAsync(
                        enrollment.BranchId,
                        cancellationToken);

                var ageGroup =
                    await _ageGroupRepository.GetByIdAsync(
                        enrollment.AgeGroupId,
                        cancellationToken);

                var batch =
                    await _batchRepository.GetByIdAsync(
                        enrollment.BatchId,
                        cancellationToken);

                seasonName = season?.Name;
                branchName = branch?.Name;
                ageGroupName = ageGroup?.Name;
                batchName = batch?.Name;

                if (enrollment.TeamId.HasValue)
                {
                    var team =
                        await _teamRepository.GetByIdAsync(
                            enrollment.TeamId.Value,
                            cancellationToken);

                    teamName = team?.Name;
                }
            }

            var parentName = parent is not null
                ? $"{parent.FirstName} {parent.LastName}"
                : "Unknown";

            playerDtos.Add(
                new PlayerDto(
                    player.Id,
                    player.PlayerNumber,

                    player.ParentId,
                    parentName,

                    player.FirstName,
                    player.LastName,
                    player.DateOfBirth,
                    CalculateAge(player.DateOfBirth),

                    player.Position.ToString(),
                    player.JerseyNumber,

                    enrollmentId,

                    seasonId,
                    seasonName,

                    branchId,
                    branchName,

                    ageGroupId,
                    ageGroupName,

                    batchId,
                    batchName,

                    teamId,
                    teamName,

                    player.Status.ToString(),
                    player.IsDeleted,

                    player.CreatedAt));
        }

        var pagedResult =
            new PagedResult<PlayerDto>(
                playerDtos,
                request.PageNumber,
                request.PageSize,
                totalCount);

        return pagedResult;
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow.Date;

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}