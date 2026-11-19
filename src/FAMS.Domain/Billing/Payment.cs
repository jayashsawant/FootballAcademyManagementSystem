using FAMS.Domain.Common;

namespace FAMS.Domain.Billing;

public class Payment : AuditableEntity
{
    public Guid InvoiceId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public Guid? RecordedBy { get; private set; }
    public string? Notes { get; private set; }

    private Payment(
        Guid invoiceId,
        decimal amount,
        PaymentMethod method,
        DateTime paymentDate,
        string? referenceNumber,
        Guid? recordedBy,
        string? notes)
    {
        InvoiceId = invoiceId;
        Amount = amount;
        Method = method;
        PaymentDate = paymentDate;
        ReferenceNumber = referenceNumber;
        RecordedBy = recordedBy;
        Notes = notes;
    }

    public static Payment Create(
        Guid invoiceId,
        decimal amount,
        PaymentMethod method,
        DateTime paymentDate,
        string? referenceNumber = null,
        Guid? recordedBy = null,
        string? notes = null)
    {
        if (invoiceId == Guid.Empty)
            throw new ArgumentException("Invoice is required.", nameof(invoiceId));

        if (amount <= 0)
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));

        return new Payment(invoiceId, amount, method, paymentDate, referenceNumber?.Trim(), recordedBy, notes?.Trim());
    }
}