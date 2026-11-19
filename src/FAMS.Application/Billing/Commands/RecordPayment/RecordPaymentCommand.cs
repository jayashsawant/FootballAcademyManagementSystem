using FAMS.Domain.Billing;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Commands.RecordPayment;

public sealed record RecordPaymentCommand(
    Guid InvoiceId,
    decimal Amount,
    PaymentMethod Method,
    DateTime PaymentDate,
    string? ReferenceNumber,
    string? Notes) : IRequest<Result<Guid>>;