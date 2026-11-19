using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Dtos;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.People;
using MediatR;

namespace FAMS.Application.People.Commands.CreatePlayer;

public sealed class CreatePlayerCommandHandler
    : IRequestHandler<CreatePlayerCommand, Result<PlayerDto>>
{
    private const string PlayerPrefix = "PLY";

    private readonly IPlayerRepository _playerRepository;
    private readonly IParentRepository _parentRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IBusinessIdGenerator _businessIdGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerCommandHandler(
        IPlayerRepository playerRepository,
        IParentRepository parentRepository,
        IBranchRepository branchRepository,
        ISeasonRepository seasonRepository,
        IAgeGroupRepository ageGroupRepository,
        IBatchRepository batchRepository,
        ITeamRepository teamRepository,
        IBusinessIdGenerator businessIdGenerator,
        IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _parentRepository = parentRepository;
        _branchRepository = branchRepository;
        _seasonRepository = seasonRepository;
        _ageGroupRepository = ageGroupRepository;
        _batchRepository = batchRepository;
        _teamRepository = teamRepository;
        _businessIdGenerator = businessIdGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PlayerDto>> Handle(
        CreatePlayerCommand request,
        CancellationToken cancellationToken)
    {
        // ---------------------------------------------------------
        // Parent
        // ---------------------------------------------------------

        var parent = await _parentRepository.GetByIdAsync(
            request.ParentId,
            cancellationToken);

        if (parent is null)
        {
            return Result<PlayerDto>.Failure(
                Error.NotFound(
                    "Player.ParentNotFound",
                    "The specified parent could not be found."));
        }

        // ---------------------------------------------------------
        // Branch
        // ---------------------------------------------------------

        var branch = await _branchRepository.GetByIdAsync(
            request.BranchId,
            cancellationToken);

        if (branch is null)
        {
            return Result<PlayerDto>.Failure(
                Error.NotFound(
                    "Player.BranchNotFound",
                    "The specified branch could not be found."));
        }

        if (!branch.IsActive)
        {
            return Result<PlayerDto>.Failure(
                Error.Conflict(
                    "Player.BranchInactive",
                    "The selected branch is inactive."));
        }

        // ---------------------------------------------------------
        // Season
        // ---------------------------------------------------------

        var season = await _seasonRepository.GetByIdAsync(
            request.SeasonId,
            cancellationToken);

        if (season is null)
        {
            return Result<PlayerDto>.Failure(
                Error.NotFound(
                    "Player.SeasonNotFound",
                    "The specified season could not be found."));
        }

        // ---------------------------------------------------------
        // Age Group
        // ---------------------------------------------------------

        var ageGroup = await _ageGroupRepository.GetByIdAsync(
            request.AgeGroupId,
            cancellationToken);

        if (ageGroup is null)
        {
            return Result<PlayerDto>.Failure(
                Error.NotFound(
                    "Player.AgeGroupNotFound",
                    "The specified age group could not be found."));
        }

        if (!ageGroup.IsActive)
        {
            return Result<PlayerDto>.Failure(
                Error.Conflict(
                    "Player.AgeGroupInactive",
                    "The selected age group is inactive."));
        }

        // ---------------------------------------------------------
        // Batch
        // ---------------------------------------------------------

        var batch = await _batchRepository.GetByIdAsync(
            request.BatchId,
            cancellationToken);

        if (batch is null)
        {
            return Result<PlayerDto>.Failure(
                Error.NotFound(
                    "Player.BatchNotFound",
                    "The specified batch could not be found."));
        }

        if (!batch.IsActive)
        {
            return Result<PlayerDto>.Failure(
                Error.Conflict(
                    "Player.BatchInactive",
                    "The selected batch is inactive."));
        }

        // ---------------------------------------------------------
        // Validate Batch relationship
        // ---------------------------------------------------------

        if (batch.BranchId != request.BranchId)
        {
            return Result<PlayerDto>.Failure(
                Error.Conflict(
                    "Player.BatchBranchMismatch",
                    "The selected batch does not belong to the selected branch."));
        }

        if (batch.SeasonId != request.SeasonId)
        {
            return Result<PlayerDto>.Failure(
                Error.Conflict(
                    "Player.BatchSeasonMismatch",
                    "The selected batch does not belong to the selected season."));
        }

        if (batch.AgeGroupId != request.AgeGroupId)
        {
            return Result<PlayerDto>.Failure(
                Error.Conflict(
                    "Player.BatchAgeGroupMismatch",
                    "The selected batch does not belong to the selected age group."));
        }

        // ---------------------------------------------------------
        // Team
        // ---------------------------------------------------------

        if (request.TeamId.HasValue)
        {
            var team = await _teamRepository.GetByIdAsync(
                request.TeamId.Value,
                cancellationToken);

            if (team is null)
            {
                return Result<PlayerDto>.Failure(
                    Error.NotFound(
                        "Player.TeamNotFound",
                        "The specified team could not be found."));
            }

            if (!team.IsActive)
            {
                return Result<PlayerDto>.Failure(
                    Error.Conflict(
                        "Player.TeamInactive",
                        "The selected team is inactive."));
            }

            if (team.BranchId != request.BranchId)
            {
                return Result<PlayerDto>.Failure(
                    Error.Conflict(
                        "Player.TeamBranchMismatch",
                        "The selected team does not belong to the selected branch."));
            }

            if (team.SeasonId != request.SeasonId)
            {
                return Result<PlayerDto>.Failure(
                    Error.Conflict(
                        "Player.TeamSeasonMismatch",
                        "The selected team does not belong to the selected season."));
            }

            if (team.AgeGroupId != request.AgeGroupId)
            {
                return Result<PlayerDto>.Failure(
                    Error.Conflict(
                        "Player.TeamAgeGroupMismatch",
                        "The selected team does not belong to the selected age group."));
            }
        }

        // ---------------------------------------------------------
        // Generate Player Number
        // ---------------------------------------------------------

        var playerNumber =
            await _businessIdGenerator.GenerateAsync(
                PlayerPrefix,
                cancellationToken);

        // ---------------------------------------------------------
        // Position
        // ---------------------------------------------------------

        var position = (PlayerPosition)request.Position;

        // ---------------------------------------------------------
        // Create Player
        // ---------------------------------------------------------

        var player = Player.Create(
            playerNumber,
            request.ParentId,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            position,
            request.JerseyNumber,
            request.MonthlyFeeOverride);

        // ---------------------------------------------------------
        // Create Initial Enrollment
        // ---------------------------------------------------------

        player.AddEnrollment(
            request.SeasonId,
            request.BranchId,
            request.AgeGroupId,
            request.BatchId,
            request.TeamId);

        // ---------------------------------------------------------
        // Persist
        // ---------------------------------------------------------

        await _playerRepository.AddAsync(
            player,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // ---------------------------------------------------------
        // DTO
        // ---------------------------------------------------------

        return new PlayerDto(
            player.Id,
            player.PlayerNumber,
            player.ParentId,
            $"{parent.FirstName} {parent.LastName}",
            player.FirstName,
            player.LastName,
            player.DateOfBirth,
            CalculateAge(player.DateOfBirth),
            player.Position.ToString(),
            player.JerseyNumber,
            null,
            request.SeasonId,
            season.Name,
            request.BranchId,
            branch.Name,
            request.AgeGroupId,
            ageGroup.Name,
            request.BatchId,
            batch.Name,
            request.TeamId,
            null,
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