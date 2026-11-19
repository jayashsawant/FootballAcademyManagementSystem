using FAMS.Domain.Common;

namespace FAMS.Domain.Content;

public class GalleryImage : AuditableEntity, IAggregateRoot
{
    public string ImageUrl { get; private set; }
    public string? Caption { get; private set; }
    public int SortOrder { get; private set; }

    private GalleryImage(string imageUrl, string? caption, int sortOrder)
    {
        ImageUrl = imageUrl;
        Caption = caption;
        SortOrder = sortOrder;
    }

    public static GalleryImage Create(string imageUrl, string? caption = null, int sortOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

        return new GalleryImage(imageUrl.Trim(), caption?.Trim(), sortOrder);
    }

    public void UpdateDetails(string imageUrl, string? caption, int sortOrder)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

        ImageUrl = imageUrl.Trim();
        Caption = caption?.Trim();
        SortOrder = sortOrder;
    }
}