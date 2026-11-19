namespace FAMS.Domain.Content;

public class PartnerLogo
{
    public Guid Id { get; private set; }
    public string LogoUrl { get; private set; } = default!;
    public string? LinkUrl { get; private set; }
    public int SortOrder { get; private set; }

    private PartnerLogo() { }

    public static PartnerLogo Create(string logoUrl, string? linkUrl, int sortOrder)
    {
        return new PartnerLogo
        {
            Id = Guid.NewGuid(),
            LogoUrl = logoUrl,
            LinkUrl = linkUrl,
            SortOrder = sortOrder
        };
    }
}