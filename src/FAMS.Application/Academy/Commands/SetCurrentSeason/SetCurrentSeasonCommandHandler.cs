using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.SetCurrentSeason;

public class SetCurrentSeasonCommandHandler : IRequestHandler<SetCurrentSeasonCommand, Result>
{
    private readonly ISeasonRepository _seasonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetCurrentSeasonCommandHandler(ISeasonRepository seasonRepository, IUnitOfWork unitOfWork)
    {
        _seasonRepository = seasonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetCurrentSeasonCommand request, CancellationToken cancellationToken)
    {
        var season = await _seasonRepository.GetByIdAsync(request.SeasonId, cancellationToken);

        if (season is null)
        {
            return Result.Failure(
                Error.NotFound("Season.NotFound", "The specified season could not be found."));
        }

        var currentSeason = await _seasonRepository.GetCurrentAsync(cancellationToken);

        currentSeason?.MarkAsNotCurrent();

        season.MarkAsCurrent();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}