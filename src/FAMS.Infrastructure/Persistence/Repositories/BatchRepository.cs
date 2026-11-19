using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public sealed class BatchRepository : IBatchRepository
{
    private readonly ApplicationDbContext _context;

    public BatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Batch?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.Batches
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);
    }

    public Task<Batch?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.Batches
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Batch>> GetAllAsync(
        Guid? branchId = null,
        Guid? seasonId = null,
        Guid? ageGroupId = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Batches
            .AsQueryable();

        if (includeInactive)
        {
            query = query.IgnoreQueryFilters();
        }

        if (!includeInactive)
        {
            query = query.Where(b => b.IsActive);
        }

        if (branchId.HasValue)
        {
            query = query.Where(
                b => b.BranchId == branchId.Value);
        }

        if (seasonId.HasValue)
        {
            query = query.Where(
                b => b.SeasonId == seasonId.Value);
        }

        if (ageGroupId.HasValue)
        {
            query = query.Where(
                b => b.AgeGroupId == ageGroupId.Value);
        }

        return await query
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> NameExistsAsync(
        Guid academyId,
        Guid branchId,
        Guid seasonId,
        Guid ageGroupId,
        string name,
        Guid? excludeBatchId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Batches
            .Where(b =>
                b.AcademyId == academyId &&
                b.BranchId == branchId &&
                b.SeasonId == seasonId &&
                b.AgeGroupId == ageGroupId &&
                b.Name == name);

        if (excludeBatchId.HasValue)
        {
            query = query.Where(
                b => b.Id != excludeBatchId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(
        Batch batch,
        CancellationToken cancellationToken = default)
    {
        await _context.Batches.AddAsync(
            batch,
            cancellationToken);
    }
}