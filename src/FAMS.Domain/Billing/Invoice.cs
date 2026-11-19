using FAMS.Domain.Common;

namespace FAMS.Domain.Billing;

public class Invoice : AuditableEntity, IAggregateRoot
{
    private readonly List<Payment> _payments = new();

    public string InvoiceNumber { get; private set; }
    public Guid PlayerId { get; private set; }
    public Guid SeasonId { get; private set; }
    public int BillingYear { get; private set; }
    public int BillingMonth { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }

    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
    public decimal AmountPaid => _payments.Sum(p => p.Amount);
    public decimal AmountDue => Amount - AmountPaid;

    private Invoice(
        string invoiceNumber,
        Guid playerId,
        Guid seasonId,
        int billingYear,
        int billingMonth,
        decimal amount,
        DateTime dueDate)
    {
        InvoiceNumber = invoiceNumber;
        PlayerId = playerId;
        SeasonId = seasonId;
        BillingYear = billingYear;
        BillingMonth = billingMonth;
        Amount = amount;
        DueDate = dueDate;
        Status = InvoiceStatus.Pending;
    }

    public static Invoice Create(
        string invoiceNumber,
        Guid playerId,
        Guid seasonId,
        int billingYear,
        int billingMonth,
        decimal amount,
        DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new ArgumentException("Invoice number cannot be empty.", nameof(invoiceNumber));

        if (playerId == Guid.Empty)
            throw new ArgumentException("Player is required.", nameof(playerId));

        if (seasonId == Guid.Empty)
            throw new ArgumentException("Season is required.", nameof(seasonId));

        if (billingMonth is < 1 or > 12)
            throw new ArgumentException("Billing month must be between 1 and 12.", nameof(billingMonth));

        if (amount <= 0)
            throw new ArgumentException("Invoice amount must be positive.", nameof(amount));

        return new Invoice(invoiceNumber.Trim(), playerId, seasonId, billingYear, billingMonth, amount, dueDate);
    }

    public Payment RecordPayment(
        decimal amount,
        PaymentMethod method,
        DateTime paymentDate,
        string? referenceNumber = null,
        Guid? recordedBy = null,
        string? notes = null)
    {
        if (Status == InvoiceStatus.Cancelled)
            throw new InvalidOperationException("Cannot record a payment against a cancelled invoice.");

        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("This invoice is already fully paid.");

        var payment = Payment.Create(Id, amount, method, paymentDate, referenceNumber, recordedBy, notes);
        _payments.Add(payment);

        RefreshStatus();

        return payment;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a fully paid invoice.");

        Status = InvoiceStatus.Cancelled;
    }

    public void RefreshOverdueStatus()
    {
        if (Status == InvoiceStatus.Pending && DateTime.UtcNow.Date > DueDate.Date)
        {
            Status = InvoiceStatus.Overdue;
        }
    }

    private void RefreshStatus()
    {
        if (AmountPaid >= Amount)
        {
            Status = InvoiceStatus.Paid;
        }
        else if (AmountPaid > 0)
        {
            Status = InvoiceStatus.PartiallyPaid;
        }
        else
        {
            Status = DateTime.UtcNow.Date > DueDate.Date ? InvoiceStatus.Overdue : InvoiceStatus.Pending;
        }
    }
}