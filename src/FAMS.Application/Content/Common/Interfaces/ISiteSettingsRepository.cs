using FAMS.Domain.Content;

namespace FAMS.Application.Content.Common.Interfaces;

public interface ISiteSettingsRepository
{
    Task<SiteSettings?> GetAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SiteSettings settings, CancellationToken cancellationToken = default);
}