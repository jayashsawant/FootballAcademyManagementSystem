using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetCurrentSeason;

public class GetCurrentSeasonQueryHandler : IRequestHandler<GetCurrentSeasonQuery, Result<SeasonDto>>
{
    private readonly ISeasonRepository _seasonRepository;

    public GetCurrentSeasonQueryHandler(ISeasonRepository seasonRepository)
    {
        _seasonRepository = seasonRepository;
    }

    public async Task<Result<SeasonDto>> Handle(GetCurrentSeasonQuery request, CancellationToken cancellationToken)
    {
        var season = await _seasonRepository.GetCurrentAsync(cancellationToken);

        if (season is null)
        {
            return Result<SeasonDto>.Failure(
                Error.NotFound("Season.NoCurrentSeason", "No season is currently marked as active."));
        }

        return new SeasonDto(season.Id, season.AcademyId, season.Name, season.StartDate, season.EndDate, season.IsCurrent);
    }
}