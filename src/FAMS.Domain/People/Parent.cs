using FAMS.Domain.Common;

namespace FAMS.Domain.People;

public class Parent : SoftDeletableEntity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }

    private Parent(string firstName, string lastName, string phone, string? email, string? address)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        Address = address;
    }

    public static Parent Create(
        string firstName,
        string lastName,
        string phone,
        string? email = null,
        string? address = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phone));

        return new Parent(firstName.Trim(), lastName.Trim(), phone.Trim(), email?.Trim(), address?.Trim());
    }

    public void UpdateDetails(string firstName, string lastName, string phone, string? email, string? address)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phone));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone.Trim();
        Email = email?.Trim();
        Address = address?.Trim();
    }

    public void Deactivate(Guid? deactivatedBy)
    {
        Delete(deactivatedBy);
    }

    public void Reactivate()
    {
        Restore();
    }
}