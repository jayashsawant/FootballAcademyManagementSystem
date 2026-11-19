using FAMS.Application.Billing.Common.Dtos;
using FAMS.Application.Billing.Common.Interfaces;
using FAMS.Application.Common.Models;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Queries.GetInvoices;

public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, Result<PagedResult<InvoiceDto>>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository, IPlayerRepository playerRepository)
    {
        _invoiceRepository = invoiceRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Result<PagedResult<InvoiceDto>>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var (invoices, totalCount) = await _invoiceRepository.SearchAsync(
            request.PlayerId,
            request.SeasonId,
            request.Status,
            request.BillingYear,
            request.BillingMonth,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        // Load players once so each invoice can carry a human-readable player name.
        var playerNameById = (await _playerRepository.GetAllAsync(cancellationToken))
            .ToDictionary(p => p.Id, p => $"{p.FirstName} {p.LastName}");

        var invoiceDtos = invoices
            .Select(invoice =>
            {
                var payments = invoice.Payments
                    .Select(p => new PaymentDto(
                        p.Id,
                        p.Amount,
                        p.Method.ToString(),
                        p.PaymentDate,
                        p.ReferenceNumber,
                        p.Notes))
                    .ToList();

                return new InvoiceDto(
                    invoice.Id,
                    invoice.InvoiceNumber,
                    invoice.PlayerId,
                    playerNameById.TryGetValue(invoice.PlayerId, out var name) ? name : "Unknown player",
                    invoice.SeasonId,
                    invoice.BillingYear,
                    invoice.BillingMonth,
                    invoice.Amount,
                    invoice.AmountPaid,
                    invoice.AmountDue,
                    invoice.DueDate,
                    invoice.Status.ToString(),
                    payments);
            })
            .ToList();

        return new PagedResult<InvoiceDto>(invoiceDtos, request.PageNumber, request.PageSize, totalCount);
    }
}
