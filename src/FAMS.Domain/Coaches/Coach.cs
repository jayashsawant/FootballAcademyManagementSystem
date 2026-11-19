using FAMS.Domain.Common;

namespace FAMS.Domain.Coaches;

public class Coach : SoftDeletableEntity, IAggregateRoot
{
    public string CoachNumber { get; private set; }
    public Guid BranchId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Email { get; private set; }
    public string Phone { get; private set; }
    public string? Qualification { get; private set; }
    public string? LicenseLevel { get; private set; }
    public int? YearsExperience { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public CoachStatus Status { get; private set; }

    private Coach(
        string coachNumber,
        Guid branchId,
        string firstName,
        string lastName,
        string? email,
        string phone,
        string? qualification,
        string? licenseLevel,
        int? yearsExperience,
        EmploymentType employmentType)
    {
        CoachNumber = coachNumber;
        BranchId = branchId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Qualification = qualification;
        LicenseLevel = licenseLevel;
        YearsExperience = yearsExperience;
        EmploymentType = employmentType;
        Status = CoachStatus.Active;
    }

    public static Coach Create(
        string coachNumber,
        Guid branchId,
        string firstName,
        string lastName,
        string phone,
        EmploymentType employmentType,
        string? email = null,
        string? qualification = null,
        string? licenseLevel = null,
        int? yearsExperience = null)
    {
        if (string.IsNullOrWhiteSpace(coachNumber))
            throw new ArgumentException("Coach number cannot be empty.", nameof(coachNumber));

        if (branchId == Guid.Empty)
            throw new ArgumentException("Branch is required.", nameof(branchId));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phone));

        if (yearsExperience is < 0)
            throw new ArgumentException("Years of experience cannot be negative.", nameof(yearsExperience));

        return new Coach(
            coachNumber,
            branchId,
            firstName.Trim(),
            lastName.Trim(),
            email?.Trim(),
            phone.Trim(),
            qualification?.Trim(),
            licenseLevel?.Trim(),
            yearsExperience,
            employmentType);
    }

    public void UpdateDetails(
        string firstName,
        string lastName,
        string phone,
        string? email,
        string? qualification,
        string? licenseLevel,
        int? yearsExperience)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phone));

        if (yearsExperience is < 0)
            throw new ArgumentException("Years of experience cannot be negative.", nameof(yearsExperience));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone.Trim();
        Email = email?.Trim();
        Qualification = qualification?.Trim();
        LicenseLevel = licenseLevel?.Trim();
        YearsExperience = yearsExperience;
    }

    public void TransferToBranch(Guid newBranchId)
    {
        if (newBranchId == Guid.Empty)
            throw new ArgumentException("Branch is required.", nameof(newBranchId));

        BranchId = newBranchId;
    }

    public void ChangeEmploymentType(EmploymentType employmentType)
    {
        EmploymentType = employmentType;
    }

    public void SetStatus(CoachStatus status)
    {
        Status = status;
    }

    public void Deactivate(Guid? deactivatedBy)
    {
        Status = CoachStatus.Terminated;
        Delete(deactivatedBy);
    }

    public void Reactivate()
    {
        Status = CoachStatus.Active;
        Restore();
    }
}