using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.People;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class ParentRepository : IParentRepository
{
    private readonly ApplicationDbContext _context;

    public ParentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Parent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Parents
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<Parent?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Parents
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<bool> PhoneExistsAsync(string phone, Guid? excludeParentId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Parents.Where(p => p.Phone == phone);

        if (excludeParentId.HasValue)
        {
            query = query.Where(p => p.Id != excludeParentId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Parent> Parents, int TotalCount)> SearchAsync(
        string? searchTerm,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Parents.IgnoreQueryFilters().AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsDeleted == !isActive.Value);
        }
        else
        {
            query = query.Where(p => !p.IsDeleted);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.FirstName.Contains(searchTerm) ||
                p.LastName.Contains(searchTerm) ||
                p.Phone.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var parents = await query
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (parents, totalCount);
    }

    public async Task AddAsync(Parent parent, CancellationToken cancellationToken = default)
    {
        await _context.Parents.AddAsync(parent, cancellationToken);
    }
}
