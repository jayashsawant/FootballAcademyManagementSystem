using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateSiteSocialLinks;

public class UpdateSiteSocialLinksCommandHandler : IRequestHandler<UpdateSiteSocialLinksCommand, Result>
{
    private readonly ISiteSettingsRepository _siteSettingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSiteSocialLinksCommandHandler(ISiteSettingsRepository siteSettingsRepository, IUnitOfWork unitOfWork)
    {
        _siteSettingsRepository = siteSettingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSiteSocialLinksCommand request, CancellationToken cancellationToken)
    {
        var settings = await _siteSettingsRepository.GetAsync(cancellationToken);

        if (settings is null)
        {
            settings = SiteSettings.CreateDefault();
            await _siteSettingsRepository.AddAsync(settings, cancellationToken);
        }

        settings.UpdateSocialLinks(request.FacebookUrl, request.InstagramUrl, request.YoutubeUrl, request.WhatsappUrl, request.LinkedinUrl);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}