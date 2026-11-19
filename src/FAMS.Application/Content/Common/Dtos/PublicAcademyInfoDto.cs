namespace FAMS.Application.Content.Common.Dtos;

public sealed record PublicAcademyInfoDto(
    string Name,
    string? Description,
    string? ContactEmail,
    string? ContactPhone,
    string? Address);