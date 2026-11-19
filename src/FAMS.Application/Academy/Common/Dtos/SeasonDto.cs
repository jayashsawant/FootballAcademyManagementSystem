namespace FAMS.Application.Academy.Common.Dtos;

public sealed record SeasonDto(
    Guid Id,
    Guid AcademyId,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsCurrent);