namespace FAMS.Application.Academy.Common.Dtos;

public sealed record GroundDto(
    Guid Id,
    Guid BranchId,
    string Name,
    string? Description,
    bool IsActive);
