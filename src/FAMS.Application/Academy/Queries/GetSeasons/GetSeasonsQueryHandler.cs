using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetSeasons;

public class GetSeasonsQueryHandler : IRequestHandler<GetSeasonsQuery, Result<IReadOnlyList<SeasonDto>>>
{
    private readonly ISeasonRepository _seasonRepository;

    public GetSeasonsQueryHandler(ISeasonRepository seasonRepository)
    {
        _seasonRepository = seasonRepository;
    }

    public async Task<Result<IReadOnlyList<SeasonDto>>> Handle(GetSeasonsQuery request, CancellationToken cancellationToken)
    {
        var seasons = await _seasonRepository.GetAllAsync(cancellationToken);

        var seasonDtos = seasons
            .Select(s => new SeasonDto(s.Id, s.AcademyId, s.Name, s.StartDate, s.EndDate, s.IsCurrent))
            .ToList();

        return seasonDtos;
    }
}