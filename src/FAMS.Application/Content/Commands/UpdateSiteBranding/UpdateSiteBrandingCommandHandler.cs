using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateSiteBranding;

public class UpdateSiteBrandingCommandHandler : IRequestHandler<UpdateSiteBrandingCommand, Result>
{
    private readonly ISiteSettingsRepository _siteSettingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSiteBrandingCommandHandler(ISiteSettingsRepository siteSettingsRepository, IUnitOfWork unitOfWork)
    {
        _siteSettingsRepository = siteSettingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSiteBrandingCommand request, CancellationToken cancellationToken)
    {
        var settings = await _siteSettingsRepository.GetAsync(cancellationToken);

        if (settings is null)
        {
            settings = SiteSettings.CreateDefault();
            await _siteSettingsRepository.AddAsync(settings, cancellationToken);
        }

        settings.UpdateBranding(request.LogoUrl, request.HeroImageUrl, request.HeroHeading, request.HeroSubtext, request.AboutText);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}