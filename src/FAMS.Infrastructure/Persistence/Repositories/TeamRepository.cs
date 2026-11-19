using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public sealed class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Team?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.Teams
            .FirstOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);
    }

    public Task<Team?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.Teams
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Team>> GetAllAsync(
        Guid? branchId = null,
        Guid? seasonId = null,
        Guid? ageGroupId = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Teams
            .AsQueryable();

        if (includeInactive)
        {
            query = query.IgnoreQueryFilters();
        }

        if (!includeInactive)
        {
            query = query.Where(t => t.IsActive);
        }

        if (branchId.HasValue)
        {
            query = query.Where(
                t => t.BranchId == branchId.Value);
        }

        if (seasonId.HasValue)
        {
            query = query.Where(
                t => t.SeasonId == seasonId.Value);
        }

        if (ageGroupId.HasValue)
        {
            query = query.Where(
                t => t.AgeGroupId == ageGroupId.Value);
        }

        return await query
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> NameExistsAsync(
        Guid academyId,
        Guid branchId,
        Guid seasonId,
        Guid ageGroupId,
        string name,
        Guid? excludeTeamId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Teams
            .Where(t =>
                t.AcademyId == academyId &&
                t.BranchId == branchId &&
                t.SeasonId == seasonId &&
                t.AgeGroupId == ageGroupId &&
                t.Name == name);

        if (excludeTeamId.HasValue)
        {
            query = query.Where(
                t => t.Id != excludeTeamId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(
        Team team,
        CancellationToken cancellationToken = default)
    {
        await _context.Teams.AddAsync(
            team,
            cancellationToken);
    }
}