using FAMS.Domain.Common;

namespace FAMS.Domain.Content;

public class TrialRequest : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string Email { get; private set; }
    public string Mobile { get; private set; }
    public TrialRequestStatus Status { get; private set; }
    public string? Notes { get; private set; }

    private TrialRequest(string name, DateTime dateOfBirth, string email, string mobile)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        Email = email;
        Mobile = mobile;
        Status = TrialRequestStatus.New;
    }

    public static TrialRequest Create(string name, DateTime dateOfBirth, string email, string mobile)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        if (dateOfBirth >= DateTime.UtcNow.Date)
            throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (string.IsNullOrWhiteSpace(mobile))
            throw new ArgumentException("Mobile cannot be empty.", nameof(mobile));

        return new TrialRequest(name.Trim(), dateOfBirth, email.Trim(), mobile.Trim());
    }

    public void UpdateStatus(TrialRequestStatus status, string? notes)
    {
        Status = status;
        Notes = notes?.Trim();
    }
}