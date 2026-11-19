using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Domain.Coaches;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class CoachRepository : ICoachRepository
{
    private readonly ApplicationDbContext _context;

    public CoachRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Coach?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Coaches.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Coach?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Coaches
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<bool> PhoneExistsAsync(string phone, Guid? excludeCoachId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Coaches.Where(c => c.Phone == phone);

        if (excludeCoachId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCoachId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Coach> Coaches, int TotalCount)> SearchAsync(
     string? searchTerm,
     Guid? branchId,
     CoachStatus? status,
     int pageNumber,
     int pageSize,
     CancellationToken cancellationToken = default)
    {
        var query = _context.Coaches.IgnoreQueryFilters().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }
        else
        {
            query = query.Where(c => c.Status != CoachStatus.Terminated);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.FirstName.Contains(searchTerm) ||
                c.LastName.Contains(searchTerm) ||
                c.CoachNumber.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm));
        }

        if (branchId.HasValue)
        {
            query = query.Where(c => c.BranchId == branchId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var coaches = await query
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (coaches, totalCount);
    }

    public async Task<IReadOnlyList<Coach>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Coaches
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Coach coach, CancellationToken cancellationToken = default)
    {
        await _context.Coaches.AddAsync(coach, cancellationToken);
    }
}
