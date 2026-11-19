using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class TrialRequestRepository : ITrialRequestRepository
{
    private readonly ApplicationDbContext _context;

    public TrialRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<TrialRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.TrialRequests.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<TrialRequest> Requests, int TotalCount)> SearchAsync(
        TrialRequestStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.TrialRequests.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var requests = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (requests, totalCount);
    }

    public async Task AddAsync(TrialRequest request, CancellationToken cancellationToken = default)
    {
        await _context.TrialRequests.AddAsync(request, cancellationToken);
    }
}