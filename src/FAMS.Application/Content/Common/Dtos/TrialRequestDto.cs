namespace FAMS.Application.Content.Common.Dtos;

public sealed record TrialRequestDto(
    Guid Id,
    string Name,
    DateTime DateOfBirth,
    string Email,
    string Mobile,
    string Status,
    string? Notes,
    DateTime CreatedAt);