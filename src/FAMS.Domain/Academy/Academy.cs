using FAMS.Domain.Common;

namespace FAMS.Domain.Academy;

public class Academy : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? Address { get; private set; }
    public decimal StandardMonthlyFee { get; private set; }

    private Academy(
        string name,
        string? description,
        string? contactEmail,
        string? contactPhone,
        string? address,
        decimal standardMonthlyFee)
    {
        Name = name;
        Description = description;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Address = address;
        StandardMonthlyFee = standardMonthlyFee;
    }

    public static Academy Create(
        string name,
        string? description = null,
        string? contactEmail = null,
        string? contactPhone = null,
        string? address = null,
        decimal standardMonthlyFee = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Academy name cannot be empty.", nameof(name));

        return new Academy(
            name.Trim(),
            description?.Trim(),
            contactEmail?.Trim(),
            contactPhone?.Trim(),
            address?.Trim(),
            standardMonthlyFee);
    }

    public void UpdateDetails(
        string name,
        string? description,
        string? contactEmail,
        string? contactPhone,
        string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Academy name cannot be empty.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        ContactEmail = contactEmail?.Trim();
        ContactPhone = contactPhone?.Trim();
        Address = address?.Trim();
    }

    public void SetStandardMonthlyFee(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Standard monthly fee cannot be negative.", nameof(amount));

        StandardMonthlyFee = amount;
    }
}