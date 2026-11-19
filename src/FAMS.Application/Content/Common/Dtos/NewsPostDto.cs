namespace FAMS.Application.Content.Common.Dtos;

public sealed record NewsPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Body,
    string? CoverImageUrl,
    string Status,
    DateTime? PublishedAt,
    DateTime CreatedAt);