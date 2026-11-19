namespace FAMS.Application.Coaches.Common.Dtos;

public sealed record CoachDto(
    Guid Id,
    string CoachNumber,
    Guid BranchId,
    string BranchName,
    string FirstName,
    string LastName,
    string? Email,
    string Phone,
    string? Qualification,
    string? LicenseLevel,
    int? YearsExperience,
    string EmploymentType,
    string Status,
    bool IsDeleted,
    DateTime CreatedAt);