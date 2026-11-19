using FAMS.Domain.Common;

namespace FAMS.Domain.Academy;

public class Batch : SoftDeletableEntity, IAggregateRoot
{
    public Guid AcademyId { get; private set; }

    public Guid BranchId { get; private set; }

    public Guid SeasonId { get; private set; }

    public Guid AgeGroupId { get; private set; }

    public string Name { get; private set; }

    public bool IsActive { get; private set; }

    private Batch(
        Guid academyId,
        Guid branchId,
        Guid seasonId,
        Guid ageGroupId,
        string name)
    {
        AcademyId = academyId;
        BranchId = branchId;
        SeasonId = seasonId;
        AgeGroupId = ageGroupId;
        Name = name;
        IsActive = true;
    }

    public static Batch Create(
        Guid academyId,
        Guid branchId,
        Guid seasonId,
        Guid ageGroupId,
        string name)
    {
        if (academyId == Guid.Empty)
            throw new ArgumentException(
                "Academy is required.",
                nameof(academyId));

        if (branchId == Guid.Empty)
            throw new ArgumentException(
                "Branch is required.",
                nameof(branchId));

        if (seasonId == Guid.Empty)
            throw new ArgumentException(
                "Season is required.",
                nameof(seasonId));

        if (ageGroupId == Guid.Empty)
            throw new ArgumentException(
                "Age group is required.",
                nameof(ageGroupId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Batch name cannot be empty.",
                nameof(name));

        return new Batch(
            academyId,
            branchId,
            seasonId,
            ageGroupId,
            name.Trim());
    }

    public void UpdateDetails(
        Guid branchId,
        Guid seasonId,
        Guid ageGroupId,
        string name)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException(
                "Branch is required.",
                nameof(branchId));

        if (seasonId == Guid.Empty)
            throw new ArgumentException(
                "Season is required.",
                nameof(seasonId));

        if (ageGroupId == Guid.Empty)
            throw new ArgumentException(
                "Age group is required.",
                nameof(ageGroupId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Batch name cannot be empty.",
                nameof(name));

        BranchId = branchId;
        SeasonId = seasonId;
        AgeGroupId = ageGroupId;
        Name = name.Trim();
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
}