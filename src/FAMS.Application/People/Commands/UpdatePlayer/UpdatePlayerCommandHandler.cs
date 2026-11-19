using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.People;
using MediatR;

namespace FAMS.Application.People.Commands.UpdatePlayer;

public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Result>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePlayerCommandHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(request.PlayerId, cancellationToken);

        if (player is null)
        {
            return Result.Failure(
                Error.NotFound("Player.NotFound", "The specified player could not be found."));
        }

        var position = (PlayerPosition)request.Position;

        player.UpdateDetails(request.FirstName, request.LastName, request.DateOfBirth, position, request.JerseyNumber);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}