using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateTeam;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);

        if (team is null)
        {
            return Result.Failure(Error.NotFound("Team.NotFound", "The specified team could not be found."));
        }

        var nameConflicts = await _teamRepository.NameExistsAsync(
            team.AcademyId, team.BranchId, team.SeasonId, team.AgeGroupId, request.Name, request.TeamId, cancellationToken);

        if (nameConflicts)
        {
            return Result.Failure(Error.Conflict("Team.NameInUse", "A team with this name already exists for this branch, season, and age group."));
        }

        team.UpdateDetails(team.BranchId, team.SeasonId, team.AgeGroupId, request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}