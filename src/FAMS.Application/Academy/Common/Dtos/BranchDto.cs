namespace FAMS.Application.Academy.Common.Dtos;

public sealed record BranchDto(
    Guid Id,
    Guid AcademyId,
    string Name,
    string? Address,
    string? City,
    string? ContactPhone,
    string? ContactEmail,
    bool IsActive,
    IReadOnlyList<GroundDto> Grounds,
    DateTime CreatedAt);
