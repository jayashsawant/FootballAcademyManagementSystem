using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.People;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public sealed class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Player?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.Players
            .Include(p => p.Enrollments)
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    public Task<Player?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.Players
            .IgnoreQueryFilters()
            .Include(p => p.Enrollments)
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Player>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Players
            .Include(p => p.Enrollments)
            .Where(p => p.Status == PlayerStatus.Active)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Player>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Players
            .Include(p => p.Enrollments)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Player> Players, int TotalCount)> SearchAsync(
        string? searchTerm,
        Guid? branchId,
        Guid? seasonId,
        Guid? ageGroupId,
        Guid? batchId,
        Guid? teamId,
        Guid? parentId,
        PlayerStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Players
            .Include(p => p.Enrollments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.FirstName.Contains(searchTerm) ||
                p.LastName.Contains(searchTerm) ||
                p.PlayerNumber.Contains(searchTerm));
        }

        if (parentId.HasValue)
        {
            query = query.Where(
                p => p.ParentId == parentId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(
                p => p.Status == status.Value);
        }

        if (branchId.HasValue)
        {
            query = query.Where(p =>
                p.Enrollments.Any(e =>
                    e.IsCurrent &&
                    e.BranchId == branchId.Value));
        }

        if (seasonId.HasValue)
        {
            query = query.Where(p =>
                p.Enrollments.Any(e =>
                    e.IsCurrent &&
                    e.SeasonId == seasonId.Value));
        }

        if (ageGroupId.HasValue)
        {
            query = query.Where(p =>
                p.Enrollments.Any(e =>
                    e.IsCurrent &&
                    e.AgeGroupId == ageGroupId.Value));
        }

        if (batchId.HasValue)
        {
            query = query.Where(p =>
                p.Enrollments.Any(e =>
                    e.IsCurrent &&
                    e.BatchId == batchId.Value));
        }

        if (teamId.HasValue)
        {
            query = query.Where(p =>
                p.Enrollments.Any(e =>
                    e.IsCurrent &&
                    e.TeamId == teamId.Value));
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var players = await query
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (players, totalCount);
    }

    public async Task AddAsync(
        Player player,
        CancellationToken cancellationToken = default)
    {
        await _context.Players.AddAsync(
            player,
            cancellationToken);
    }
}