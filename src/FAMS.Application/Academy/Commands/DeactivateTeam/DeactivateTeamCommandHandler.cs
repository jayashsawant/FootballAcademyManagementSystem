using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateTeam;

public class DeactivateTeamCommandHandler : IRequestHandler<DeactivateTeamCommand, Result>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateTeamCommandHandler(
        ITeamRepository teamRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);

        if (team is null)
        {
            return Result.Failure(Error.NotFound("Team.NotFound", "The specified team could not be found."));
        }

        if (!team.IsActive)
        {
            return Result.Failure(Error.Conflict("Team.AlreadyInactive", "This team is already deactivated."));
        }

        team.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}