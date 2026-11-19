namespace FAMS.Application.Billing.Common.Dtos;

public sealed record PaymentDto(
    Guid Id,
    decimal Amount,
    string Method,
    DateTime PaymentDate,
    string? ReferenceNumber,
    string? Notes);