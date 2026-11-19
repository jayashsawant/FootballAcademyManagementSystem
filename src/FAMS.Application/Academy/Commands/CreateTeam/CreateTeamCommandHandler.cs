using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Academy;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateTeam;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<TeamDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamCommandHandler(
        ITeamRepository teamRepository,
        IBranchRepository branchRepository,
        ISeasonRepository seasonRepository,
        IAgeGroupRepository ageGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _branchRepository = branchRepository;
        _seasonRepository = seasonRepository;
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TeamDto>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<TeamDto>.Failure(
                Error.NotFound("Team.BranchNotFound", "The specified branch could not be found."));
        }

        var season = await _seasonRepository.GetByIdAsync(request.SeasonId, cancellationToken);

        if (season is null)
        {
            return Result<TeamDto>.Failure(
                Error.NotFound("Team.SeasonNotFound", "The specified season could not be found."));
        }

        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.AgeGroupId, cancellationToken);

        if (ageGroup is null)
        {
            return Result<TeamDto>.Failure(
                Error.NotFound("Team.AgeGroupNotFound", "The specified age group could not be found."));
        }

        if (ageGroup.AcademyId != branch.AcademyId)
        {
            return Result<TeamDto>.Failure(
                Error.Conflict("Team.AgeGroupMismatch", "The specified age group does not belong to this branch's academy."));
        }

        var nameExists = await _teamRepository.NameExistsAsync(
            branch.AcademyId, request.BranchId, request.SeasonId, request.AgeGroupId, request.Name,
            cancellationToken: cancellationToken);

        if (nameExists)
        {
            return Result<TeamDto>.Failure(
                Error.Conflict("Team.NameInUse", "A team with this name already exists for this branch, season, and age group."));
        }

        var team = Team.Create(branch.AcademyId, request.BranchId, request.SeasonId, request.AgeGroupId, request.Name);

        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TeamDto(
            team.Id, team.BranchId, branch.Name, team.SeasonId, season.Name,
            team.AgeGroupId, ageGroup.Name, team.Name, team.IsActive);
    }
}