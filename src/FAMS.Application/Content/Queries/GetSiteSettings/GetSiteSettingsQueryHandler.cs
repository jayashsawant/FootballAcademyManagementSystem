using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetSiteSettings;

public class GetSiteSettingsQueryHandler : IRequestHandler<GetSiteSettingsQuery, Result<SiteSettingsDto>>
{
    private readonly ISiteSettingsRepository _siteSettingsRepository;

    public GetSiteSettingsQueryHandler(ISiteSettingsRepository siteSettingsRepository)
    {
        _siteSettingsRepository = siteSettingsRepository;
    }

    public async Task<Result<SiteSettingsDto>> Handle(GetSiteSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _siteSettingsRepository.GetAsync(cancellationToken);

        if (settings is null)
        {
            return new SiteSettingsDto(Guid.Empty, null, null, null, null, null, null, null, null, null, null);
        }

        return new SiteSettingsDto(
            settings.Id, settings.LogoUrl, settings.HeroImageUrl, settings.HeroHeading, settings.HeroSubtext,
            settings.AboutText, settings.FacebookUrl, settings.InstagramUrl, settings.YoutubeUrl,
            settings.WhatsappUrl, settings.LinkedinUrl);
    }
}