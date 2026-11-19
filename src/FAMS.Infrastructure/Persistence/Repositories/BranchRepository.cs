using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly ApplicationDbContext _context;

    public BranchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Branches
            .Include(b => b.Grounds)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Branch>> GetAllAsync(bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Branches
            .Include(b => b.Grounds)
            .AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(b => b.IsActive);
        }

        return await query.OrderBy(b => b.Name).ToListAsync(cancellationToken);
    }

    public Task<bool> NameExistsAsync(Guid academyId, string name, Guid? excludeBranchId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Branches
            .Where(b => b.AcademyId == academyId && b.Name == name);

        if (excludeBranchId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBranchId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public Task<Branch?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Branches
            .IgnoreQueryFilters()
            .Include(b => b.Grounds)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddGroundAsync(Ground ground, CancellationToken cancellationToken = default)
    {
        await _context.Grounds.AddAsync(ground, cancellationToken);
    }

    public async Task AddAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        await _context.Branches.AddAsync(branch, cancellationToken);
    }
}
