namespace FAMS.Application.Academy.Common.Dtos;

public sealed record BatchDto(
    Guid Id,
    Guid BranchId,
    string BranchName,
    Guid SeasonId,
    string SeasonName,
    Guid AgeGroupId,
    string AgeGroupName,
    string Name,
    bool IsActive);
