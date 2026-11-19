using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class SiteSettingsRepository : ISiteSettingsRepository
{
    private readonly ApplicationDbContext _context;

    public SiteSettingsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<SiteSettings?> GetAsync(CancellationToken cancellationToken = default)
    {
        return _context.SiteSettings.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(SiteSettings settings, CancellationToken cancellationToken = default)
    {
        await _context.SiteSettings.AddAsync(settings, cancellationToken);
    }
}