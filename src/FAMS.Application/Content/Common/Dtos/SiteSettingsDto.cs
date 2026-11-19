namespace FAMS.Application.Content.Common.Dtos;

public sealed record SiteSettingsDto(
    Guid Id,
    string? LogoUrl,
    string? HeroImageUrl,
    string? HeroHeading,
    string? HeroSubtext,
    string? AboutText,
    string? FacebookUrl,
    string? InstagramUrl,
    string? YoutubeUrl,
    string? WhatsappUrl,
    string? LinkedinUrl);