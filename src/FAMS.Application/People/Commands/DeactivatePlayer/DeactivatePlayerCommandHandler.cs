using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.DeactivatePlayer;

public class DeactivatePlayerCommandHandler : IRequestHandler<DeactivatePlayerCommand, Result>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivatePlayerCommandHandler(
        IPlayerRepository playerRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(request.PlayerId, cancellationToken);

        if (player is null)
        {
            return Result.Failure(
                Error.NotFound("Player.NotFound", "The specified player could not be found."));
        }

        if (player.Status == Domain.People.PlayerStatus.Inactive)
        {
            return Result.Failure(
                Error.Conflict("Player.AlreadyInactive", "This player is already inactive."));
        }

        player.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}