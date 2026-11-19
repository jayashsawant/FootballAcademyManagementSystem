using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.People.Common.Dtos;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetPlayerById;

public sealed class GetPlayerByIdQueryHandler
    : IRequestHandler<GetPlayerByIdQuery, Result<PlayerDto>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IParentRepository _parentRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly ITeamRepository _teamRepository;

    public GetPlayerByIdQueryHandler(
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

    public async Task<Result<PlayerDto>> Handle(
        GetPlayerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(
            request.PlayerId,
            cancellationToken);

        if (player is null)
        {
            return Result<PlayerDto>.Failure(
                Error.NotFound(
                    "Player.NotFound",
                    "The specified player could not be found."));
        }

        var parent = await _parentRepository.GetByIdAsync(
            player.ParentId,
            cancellationToken);

        var enrollment = player.Enrollments
            .FirstOrDefault(e => e.IsCurrent);

        string? seasonName = null;
        string? branchName = null;
        string? ageGroupName = null;
        string? batchName = null;
        string? teamName = null;

        Guid? seasonId = null;
        Guid? branchId = null;
        Guid? ageGroupId = null;
        Guid? batchId = null;
        Guid? teamId = null;
        Guid? enrollmentId = null;

        if (enrollment is not null)
        {
            enrollmentId = enrollment.Id;

            seasonId = enrollment.SeasonId;
            branchId = enrollment.BranchId;
            ageGroupId = enrollment.AgeGroupId;
            batchId = enrollment.BatchId;
            teamId = enrollment.TeamId;

            var season = await _seasonRepository.GetByIdAsync(
                enrollment.SeasonId,
                cancellationToken);

            var branch = await _branchRepository.GetByIdAsync(
                enrollment.BranchId,
                cancellationToken);

            var ageGroup = await _ageGroupRepository.GetByIdAsync(
                enrollment.AgeGroupId,
                cancellationToken);

            var batch = await _batchRepository.GetByIdAsync(
                enrollment.BatchId,
                cancellationToken);

            seasonName = season?.Name;
            branchName = branch?.Name;
            ageGroupName = ageGroup?.Name;
            batchName = batch?.Name;

            if (enrollment.TeamId.HasValue)
            {
                var team = await _teamRepository.GetByIdAsync(
                    enrollment.TeamId.Value,
                    cancellationToken);

                teamName = team?.Name;
            }
        }

        var parentName = parent is not null
            ? $"{parent.FirstName} {parent.LastName}"
            : "Unknown";

        return new PlayerDto(
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

            player.CreatedAt);
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