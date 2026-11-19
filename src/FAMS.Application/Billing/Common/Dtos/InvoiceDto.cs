namespace FAMS.Application.Billing.Common.Dtos;

public sealed record InvoiceDto(
    Guid Id,
    string InvoiceNumber,
    Guid PlayerId,
    string PlayerName,
    Guid SeasonId,
    int BillingYear,
    int BillingMonth,
    decimal Amount,
    decimal AmountPaid,
    decimal AmountDue,
    DateTime DueDate,
    string Status,
    IReadOnlyList<PaymentDto> Payments);
