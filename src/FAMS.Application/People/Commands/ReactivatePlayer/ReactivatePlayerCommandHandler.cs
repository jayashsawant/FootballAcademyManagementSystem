using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.ReactivatePlayer;

public class ReactivatePlayerCommandHandler : IRequestHandler<ReactivatePlayerCommand, Result>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivatePlayerCommandHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdIncludingDeletedAsync(request.PlayerId, cancellationToken);

        if (player is null)
        {
            return Result.Failure(
                Error.NotFound("Player.NotFound", "The specified player could not be found."));
        }

        if (!player.IsDeleted)
        {
            return Result.Failure(
                Error.Conflict("Player.AlreadyActive", "This player is already active."));
        }

        player.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}