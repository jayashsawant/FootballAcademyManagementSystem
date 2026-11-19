namespace FAMS.Application.Academy.Common.Dtos;

public sealed record AgeGroupDto(
    Guid Id,
    Guid AcademyId,
    string Name,
    int? MinimumAge,
    int? MaximumAge,
    int SortOrder,
    bool IsActive);