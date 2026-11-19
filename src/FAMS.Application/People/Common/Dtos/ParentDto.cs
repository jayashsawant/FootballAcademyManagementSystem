namespace FAMS.Application.People.Common.Dtos;

public sealed record ParentDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string? Email,
    string? Address,
    bool IsDeleted,
    DateTime CreatedAt,
    IReadOnlyList<PlayerSummaryDto> Children);
