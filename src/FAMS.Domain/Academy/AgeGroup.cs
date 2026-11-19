using FAMS.Domain.Common;

namespace FAMS.Domain.Academy;

public class AgeGroup : SoftDeletableEntity, IAggregateRoot
{
    public Guid AcademyId { get; private set; }

    public string Name { get; private set; }

    public int? MinimumAge { get; private set; }

    public int? MaximumAge { get; private set; }

    public int SortOrder { get; private set; }

    public bool IsActive { get; private set; }

    private AgeGroup(
        Guid academyId,
        string name,
        int? minimumAge,
        int? maximumAge,
        int sortOrder)
    {
        AcademyId = academyId;
        Name = name;
        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
        SortOrder = sortOrder;
        IsActive = true;
    }

    public static AgeGroup Create(
        Guid academyId,
        string name,
        int? minimumAge = null,
        int? maximumAge = null,
        int sortOrder = 0)
    {
        if (academyId == Guid.Empty)
            throw new ArgumentException(
                "Academy is required.",
                nameof(academyId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Age group name cannot be empty.",
                nameof(name));

        ValidateAgeRange(minimumAge, maximumAge);

        if (sortOrder < 0)
            throw new ArgumentException(
                "Sort order cannot be negative.",
                nameof(sortOrder));

        return new AgeGroup(
            academyId,
            name.Trim(),
            minimumAge,
            maximumAge,
            sortOrder);
    }

    public void UpdateDetails(
        string name,
        int? minimumAge,
        int? maximumAge,
        int sortOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Age group name cannot be empty.",
                nameof(name));

        ValidateAgeRange(minimumAge, maximumAge);

        if (sortOrder < 0)
            throw new ArgumentException(
                "Sort order cannot be negative.",
                nameof(sortOrder));

        Name = name.Trim();
        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
        SortOrder = sortOrder;
    }

    public void Deactivate(Guid? deactivatedBy)
    {
        IsActive = false;
        Delete(deactivatedBy);
    }

    public void Reactivate()
    {
        IsActive = true;
        Restore();
    }

    private static void ValidateAgeRange(
        int? minimumAge,
        int? maximumAge)
    {
        if (minimumAge is < 0)
            throw new ArgumentException(
                "Minimum age cannot be negative.",
                nameof(minimumAge));

        if (maximumAge is < 0)
            throw new ArgumentException(
                "Maximum age cannot be negative.",
                nameof(maximumAge));

        if (minimumAge.HasValue &&
            maximumAge.HasValue &&
            minimumAge.Value > maximumAge.Value)
        {
            throw new ArgumentException(
                "Minimum age cannot be greater than maximum age.");
        }
    }
}