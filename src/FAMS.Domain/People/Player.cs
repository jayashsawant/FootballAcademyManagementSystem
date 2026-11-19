using FAMS.Domain.Common;

namespace FAMS.Domain.People;

public class Player : SoftDeletableEntity, IAggregateRoot
{
    private readonly List<PlayerEnrollment> _enrollments = new();

    public string PlayerNumber { get; private set; }

    public Guid ParentId { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public PlayerPosition Position { get; private set; }

    public int? JerseyNumber { get; private set; }

    public PlayerStatus Status { get; private set; }

    public decimal? MonthlyFeeOverride { get; private set; }

    public IReadOnlyCollection<PlayerEnrollment> Enrollments =>
        _enrollments.AsReadOnly();

    private Player(
        string playerNumber,
        Guid parentId,
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        PlayerPosition position,
        int? jerseyNumber,
        decimal? monthlyFeeOverride = null)
    {
        PlayerNumber = playerNumber;
        ParentId = parentId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Position = position;
        JerseyNumber = jerseyNumber;
        MonthlyFeeOverride = monthlyFeeOverride;
        Status = PlayerStatus.Active;
    }

    public static Player Create(
        string playerNumber,
        Guid parentId,
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        PlayerPosition position,
        int? jerseyNumber = null,
        decimal? monthlyFeeOverride = null)
    {
        if (string.IsNullOrWhiteSpace(playerNumber))
            throw new ArgumentException(
                "Player number cannot be empty.",
                nameof(playerNumber));

        if (parentId == Guid.Empty)
            throw new ArgumentException(
                "Parent is required.",
                nameof(parentId));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException(
                "First name cannot be empty.",
                nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException(
                "Last name cannot be empty.",
                nameof(lastName));

        if (dateOfBirth >= DateTime.UtcNow.Date)
            throw new ArgumentException(
                "Date of birth must be in the past.",
                nameof(dateOfBirth));

        if (jerseyNumber is < 1)
            throw new ArgumentException(
                "Jersey number must be positive.",
                nameof(jerseyNumber));

        if (monthlyFeeOverride is < 0)
            throw new ArgumentException(
                "Fee override cannot be negative.",
                nameof(monthlyFeeOverride));

        return new Player(
            playerNumber,
            parentId,
            firstName.Trim(),
            lastName.Trim(),
            dateOfBirth,
            position,
            jerseyNumber,
            monthlyFeeOverride);
    }

    public void UpdateDetails(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        PlayerPosition position,
        int? jerseyNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException(
                "First name cannot be empty.",
                nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException(
                "Last name cannot be empty.",
                nameof(lastName));

        if (dateOfBirth >= DateTime.UtcNow.Date)
            throw new ArgumentException(
                "Date of birth must be in the past.",
                nameof(dateOfBirth));

        if (jerseyNumber is < 1)
            throw new ArgumentException(
                "Jersey number must be positive.",
                nameof(jerseyNumber));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        Position = position;
        JerseyNumber = jerseyNumber;
    }

    public void SetStatus(PlayerStatus status)
    {
        Status = status;
    }

    public void Deactivate(Guid? deactivatedBy)
    {
        Status = PlayerStatus.Inactive;
        Delete(deactivatedBy);
    }

    public void Reactivate()
    {
        Status = PlayerStatus.Active;
        Restore();
    }

    public void SetFeeOverride(decimal? amount)
    {
        if (amount is < 0)
            throw new ArgumentException(
                "Fee override cannot be negative.",
                nameof(amount));

        MonthlyFeeOverride = amount;
    }

    public PlayerEnrollment AddEnrollment(
        Guid seasonId,
        Guid branchId,
        Guid ageGroupId,
        Guid batchId,
        Guid? teamId = null,
        DateTime? enrollmentDate = null)
    {
        var existingCurrentEnrollment = _enrollments
            .FirstOrDefault(e => e.IsCurrent);

        if (existingCurrentEnrollment is not null)
        {
            throw new InvalidOperationException(
                "The player already has a current enrollment.");
        }

        var enrollment = PlayerEnrollment.Create(
            Id,
            seasonId,
            branchId,
            ageGroupId,
            batchId,
            teamId,
            enrollmentDate);

        _enrollments.Add(enrollment);

        return enrollment;
    }
}