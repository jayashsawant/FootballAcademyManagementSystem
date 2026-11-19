namespace FAMS.Application.Academy.Common.Dtos;

public sealed record AcademyDto(
    Guid Id,
    string Name,
    string? Description,
    string? ContactEmail,
    string? ContactPhone,
    string? Address);