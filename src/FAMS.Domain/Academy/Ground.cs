using FAMS.Domain.Common;

namespace FAMS.Domain.Academy;

public class Ground : AuditableEntity
{
    public Guid BranchId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private Ground(Guid branchId, string name, string? description)
    {
        BranchId = branchId;
        Name = name;
        Description = description;
        IsActive = true;
    }

    public static Ground Create(Guid branchId, string name, string? description = null)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException("Branch is required.", nameof(branchId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ground name cannot be empty.", nameof(name));

        return new Ground(branchId, name.Trim(), description?.Trim());
    }

    public void UpdateDetails(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ground name cannot be empty.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Reactivate()
    {
        IsActive = true;
    }
}