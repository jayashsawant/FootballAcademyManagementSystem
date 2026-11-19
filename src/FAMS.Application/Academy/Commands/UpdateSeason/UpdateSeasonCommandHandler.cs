using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateSeason;

public class UpdateSeasonCommandHandler : IRequestHandler<UpdateSeasonCommand, Result>
{
    private readonly ISeasonRepository _seasonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSeasonCommandHandler(ISeasonRepository seasonRepository, IUnitOfWork unitOfWork)
    {
        _seasonRepository = seasonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSeasonCommand request, CancellationToken cancellationToken)
    {
        var season = await _seasonRepository.GetByIdAsync(request.SeasonId, cancellationToken);

        if (season is null)
        {
            return Result.Failure(
                Error.NotFound("Season.NotFound", "The specified season could not be found."));
        }

        season.UpdateDetails(request.Name, request.StartDate, request.EndDate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}