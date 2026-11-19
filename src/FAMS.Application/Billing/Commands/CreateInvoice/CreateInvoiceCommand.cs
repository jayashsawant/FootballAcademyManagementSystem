using FAMS.Application.Billing.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Commands.CreateInvoice;

public sealed record CreateInvoiceCommand(
    Guid PlayerId,
    Guid SeasonId,
    int BillingYear,
    int BillingMonth,
    decimal Amount,
    DateTime DueDate) : IRequest<Result<InvoiceDto>>;