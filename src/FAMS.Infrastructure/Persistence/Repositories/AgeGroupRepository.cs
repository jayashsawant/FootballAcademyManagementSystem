using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public sealed class AgeGroupRepository : IAgeGroupRepository
{
    private readonly ApplicationDbContext _context;

    public AgeGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<AgeGroup?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.AgeGroups
            .FirstOrDefaultAsync(
                a => a.Id == id,
                cancellationToken);
    }

    public Task<AgeGroup?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.AgeGroups
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                a => a.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<AgeGroup>> GetAllAsync(
        Guid academyId,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        if (academyId == Guid.Empty)
        {
            throw new ArgumentException(
                "Academy is required.",
                nameof(academyId));
        }

        IQueryable<AgeGroup> query = _context.AgeGroups;

        /*
         * When includeInactive = true, we need to bypass the
         * global soft-delete filter so that inactive/deleted
         * age groups can also be returned.
         */
        if (includeInactive)
        {
            query = query.IgnoreQueryFilters();
        }

        query = query.Where(a => a.AcademyId == academyId);

        /*
         * When inactive records are not requested, explicitly
         * restrict the result to active age groups.
         *
         * This is intentionally kept even though the global
         * soft-delete filter already hides deleted records.
         */
        if (!includeInactive)
        {
            query = query.Where(a => a.IsActive);
        }

        return await query
            .OrderBy(a => a.SortOrder)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> NameExistsAsync(
        Guid academyId,
        string name,
        Guid? excludeAgeGroupId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AgeGroups
            .Where(a =>
                a.AcademyId == academyId &&
                a.Name == name);

        if (excludeAgeGroupId.HasValue)
        {
            query = query.Where(
                a => a.Id != excludeAgeGroupId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(
        AgeGroup ageGroup,
        CancellationToken cancellationToken = default)
    {
        await _context.AgeGroups.AddAsync(
            ageGroup,
            cancellationToken);
    }
}