using FAMS.Domain.Common;
using System.Text.RegularExpressions;

namespace FAMS.Domain.Academy;

public class Branch : SoftDeletableEntity, IAggregateRoot
{
    private readonly List<Ground> _grounds = new();

    public Guid AcademyId { get; private set; }
    public string Name { get; private set; }
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? ContactEmail { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<Ground> Grounds => _grounds.AsReadOnly();

    private Branch(
        Guid academyId,
        string name,
        string? address,
        string? city,
        string? contactPhone,
        string? contactEmail)
    {
        AcademyId = academyId;
        Name = name;
        Address = address;
        City = city;
        ContactPhone = contactPhone;
        ContactEmail = contactEmail;
        IsActive = true;
    }

    public static Branch Create(
        Guid academyId,
        string name,
        string? address = null,
        string? city = null,
        string? contactPhone = null,
        string? contactEmail = null)
    {
        if (academyId == Guid.Empty)
            throw new ArgumentException("Academy is required.", nameof(academyId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Branch name cannot be empty.", nameof(name));

        return new Branch(
            academyId,
            name.Trim(),
            address?.Trim(),
            city?.Trim(),
            contactPhone?.Trim(),
            contactEmail?.Trim());
    }

    public void UpdateDetails(
        string name,
        string? address,
        string? city,
        string? contactPhone,
        string? contactEmail)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Branch name cannot be empty.", nameof(name));

        Name = name.Trim();
        Address = address?.Trim();
        City = city?.Trim();
        ContactPhone = contactPhone?.Trim();
        ContactEmail = contactEmail?.Trim();
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

    public Ground AddGround(string name, string? description)
    {
        var ground = Ground.Create(Id, name, description);
        _grounds.Add(ground);
        return ground;
    }

    public void RemoveGround(Guid groundId)
    {
        var ground = _grounds.FirstOrDefault(g => g.Id == groundId);

        if (ground is null)
            throw new InvalidOperationException("The specified ground does not belong to this branch.");

        _grounds.Remove(ground);
    }
}