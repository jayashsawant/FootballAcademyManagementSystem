using FAMS.Domain.Billing;

namespace FAMS.Application.Billing.Common.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsForPeriodAsync(Guid playerId, int billingYear, int billingMonth, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Invoice> Invoices, int TotalCount)> SearchAsync(
        Guid? playerId,
        Guid? seasonId,
        InvoiceStatus? status,
        int? billingYear,
        int? billingMonth,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);

    Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default);
}
