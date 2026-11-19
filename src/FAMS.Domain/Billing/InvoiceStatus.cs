namespace FAMS.Domain.Billing;

public enum InvoiceStatus
{
    Pending,
    PartiallyPaid,
    Paid,
    Overdue,
    Cancelled
}