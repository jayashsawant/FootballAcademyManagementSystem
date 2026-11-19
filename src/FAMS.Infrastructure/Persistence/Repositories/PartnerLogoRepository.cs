using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class PartnerLogoRepository : IPartnerLogoRepository
{
    private readonly ApplicationDbContext _context;

    public PartnerLogoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<PartnerLogo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.PartnerLogos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<PartnerLogo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PartnerLogos.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PartnerLogo logo, CancellationToken cancellationToken = default)
    {
        await _context.PartnerLogos.AddAsync(logo, cancellationToken);
    }

    public Task RemoveAsync(PartnerLogo logo, CancellationToken cancellationToken = default)
    {
        _context.PartnerLogos.Remove(logo);
        return Task.CompletedTask;
    }
}