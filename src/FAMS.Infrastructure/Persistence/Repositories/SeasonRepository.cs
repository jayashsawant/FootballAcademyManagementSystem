using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class SeasonRepository : ISeasonRepository
{
    private readonly ApplicationDbContext _context;

    public SeasonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Season?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Seasons.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Seasons
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);
    }

    public Task<Season?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        return _context.Seasons.FirstOrDefaultAsync(s => s.IsCurrent, cancellationToken);
    }

    public async Task AddAsync(Season season, CancellationToken cancellationToken = default)
    {
        await _context.Seasons.AddAsync(season, cancellationToken);
    }
}