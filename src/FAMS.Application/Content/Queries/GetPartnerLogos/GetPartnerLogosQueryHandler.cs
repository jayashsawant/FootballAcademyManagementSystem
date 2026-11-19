using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPartnerLogos;

public class GetPartnerLogosQueryHandler : IRequestHandler<GetPartnerLogosQuery, Result<IReadOnlyList<PartnerLogoDto>>>
{
    private readonly IPartnerLogoRepository _partnerLogoRepository;

    public GetPartnerLogosQueryHandler(IPartnerLogoRepository partnerLogoRepository)
    {
        _partnerLogoRepository = partnerLogoRepository;
    }

    public async Task<Result<IReadOnlyList<PartnerLogoDto>>> Handle(GetPartnerLogosQuery request, CancellationToken cancellationToken)
    {
        var logos = await _partnerLogoRepository.GetAllAsync(cancellationToken);

        var dtos = logos
            .OrderBy(l => l.SortOrder)
            .Select(l => new PartnerLogoDto(l.Id, l.LogoUrl, l.LinkUrl, l.SortOrder))
            .ToList();

        return dtos;
    }
}