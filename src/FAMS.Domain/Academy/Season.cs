using FAMS.Domain.Common;

namespace FAMS.Domain.Academy;

public class Season : AuditableEntity, IAggregateRoot
{
    public Guid AcademyId { get; private set; }
    public string Name { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsCurrent { get; private set; }

    private Season(Guid academyId, string name, DateTime startDate, DateTime endDate)
    {
        AcademyId = academyId;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        IsCurrent = false;
    }

    public static Season Create(Guid academyId, string name, DateTime startDate, DateTime endDate)
    {
        if (academyId == Guid.Empty)
            throw new ArgumentException("Academy is required.", nameof(academyId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Season name cannot be empty.", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after the start date.", nameof(endDate));

        return new Season(academyId, name.Trim(), startDate, endDate);
    }

    public void UpdateDetails(string name, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Season name cannot be empty.", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after the start date.", nameof(endDate));

        Name = name.Trim();
        StartDate = startDate;
        EndDate = endDate;
    }

    public void MarkAsCurrent()
    {
        IsCurrent = true;
    }

    public void MarkAsNotCurrent()
    {
        IsCurrent = false;
    }
}