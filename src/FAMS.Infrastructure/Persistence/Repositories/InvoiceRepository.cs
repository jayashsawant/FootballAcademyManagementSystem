using FAMS.Application.Billing.Common.Interfaces;
using FAMS.Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Invoices
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public Task<bool> ExistsForPeriodAsync(Guid playerId, int billingYear, int billingMonth, CancellationToken cancellationToken = default)
    {
        return _context.Invoices.AnyAsync(
            i => i.PlayerId == playerId && i.BillingYear == billingYear && i.BillingMonth == billingMonth,
            cancellationToken);
    }

    public async Task<(IReadOnlyList<Invoice> Invoices, int TotalCount)> SearchAsync(
        Guid? playerId,
        Guid? seasonId,
        InvoiceStatus? status,
        int? billingYear,
        int? billingMonth,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Invoices.Include(i => i.Payments).AsQueryable();

        if (playerId.HasValue) query = query.Where(i => i.PlayerId == playerId.Value);
        if (seasonId.HasValue) query = query.Where(i => i.SeasonId == seasonId.Value);
        if (status.HasValue) query = query.Where(i => i.Status == status.Value);
        if (billingYear.HasValue) query = query.Where(i => i.BillingYear == billingYear.Value);
        if (billingMonth.HasValue) query = query.Where(i => i.BillingMonth == billingMonth.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var invoices = await query
            .OrderByDescending(i => i.BillingYear)
            .ThenByDescending(i => i.BillingMonth)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (invoices, totalCount);
    }

    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        await _context.Invoices.AddAsync(invoice, cancellationToken);
    }

    public async Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // Explicitly track as Added — relying on DetectChanges to discover a new
        // Payment inside Invoice._payments tracked it as Modified (UPDATE on a
        // non-existent row), which failed SaveChanges with a concurrency error.
        await _context.Payments.AddAsync(payment, cancellationToken);
    }
}
