using FAMS.Domain.Common;

namespace FAMS.Domain.Content;

public class NewsPost : AuditableEntity, IAggregateRoot
{
    public string Title { get; private set; }
    public string Slug { get; private set; }
    public string Body { get; private set; }
    public string? CoverImageUrl { get; private set; }
    public NewsPostStatus Status { get; private set; }
    public DateTime? PublishedAt { get; private set; }

    private NewsPost(string title, string slug, string body, string? coverImageUrl)
    {
        Title = title;
        Slug = slug;
        Body = body;
        CoverImageUrl = coverImageUrl;
        Status = NewsPostStatus.Draft;
    }

    public static NewsPost Create(string title, string slug, string body, string? coverImageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty.", nameof(slug));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body cannot be empty.", nameof(body));

        return new NewsPost(title.Trim(), slug.Trim().ToLowerInvariant(), body, coverImageUrl?.Trim());
    }

    public void UpdateContent(string title, string body, string? coverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body cannot be empty.", nameof(body));

        Title = title.Trim();
        Body = body;
        CoverImageUrl = coverImageUrl?.Trim();
    }

    public void Publish()
    {
        Status = NewsPostStatus.Published;
        PublishedAt = DateTime.UtcNow;
    }

    public void Unpublish()
    {
        Status = NewsPostStatus.Draft;
        PublishedAt = null;
    }
}