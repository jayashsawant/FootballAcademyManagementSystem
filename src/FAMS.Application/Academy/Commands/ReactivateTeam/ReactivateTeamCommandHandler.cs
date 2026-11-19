using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateTeam;

public class ReactivateTeamCommandHandler : IRequestHandler<ReactivateTeamCommand, Result>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdIncludingDeletedAsync(request.TeamId, cancellationToken);

        if (team is null)
        {
            return Result.Failure(Error.NotFound("Team.NotFound", "The specified team could not be found."));
        }

        if (team.IsActive)
        {
            return Result.Failure(Error.Conflict("Team.AlreadyActive", "This team is already active."));
        }

        team.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}