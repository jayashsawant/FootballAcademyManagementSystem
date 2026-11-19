using FAMS.Domain.Common;

namespace FAMS.Domain.Content;

public class SiteSettings : AuditableEntity, IAggregateRoot
{
    public string? LogoUrl { get; private set; }
    public string? HeroImageUrl { get; private set; }
    public string? HeroHeading { get; private set; }
    public string? HeroSubtext { get; private set; }
    public string? AboutText { get; private set; }
    public string? FacebookUrl { get; private set; }
    public string? InstagramUrl { get; private set; }
    public string? YoutubeUrl { get; private set; }
    public string? WhatsappUrl { get; private set; }
    public string? LinkedinUrl { get; private set; }

    private SiteSettings() { }

    public static SiteSettings CreateDefault()
    {
        return new SiteSettings();
    }

    public void UpdateBranding(string? logoUrl, string? heroImageUrl, string? heroHeading, string? heroSubtext, string? aboutText)
    {
        LogoUrl = logoUrl?.Trim();
        HeroImageUrl = heroImageUrl?.Trim();
        HeroHeading = heroHeading?.Trim();
        HeroSubtext = heroSubtext?.Trim();
        AboutText = aboutText?.Trim();
    }

    public void UpdateSocialLinks(string? facebookUrl, string? instagramUrl, string? youtubeUrl, string? whatsappUrl, string? linkedinUrl)
    {
        FacebookUrl = facebookUrl?.Trim();
        InstagramUrl = instagramUrl?.Trim();
        YoutubeUrl = youtubeUrl?.Trim();
        WhatsappUrl = whatsappUrl?.Trim();
        LinkedinUrl = linkedinUrl?.Trim();
    }
}