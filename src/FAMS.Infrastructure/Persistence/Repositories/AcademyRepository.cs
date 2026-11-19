using FAMS.Application.Academy.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class AcademyRepository : IAcademyRepository
{
    private readonly ApplicationDbContext _context;

    public AcademyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Domain.Academy.Academy?> GetAsync(CancellationToken cancellationToken = default)
    {
        return _context.Academies.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
    {
        return _context.Academies.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Domain.Academy.Academy academy, CancellationToken cancellationToken = default)
    {
        await _context.Academies.AddAsync(academy, cancellationToken);
    }
}