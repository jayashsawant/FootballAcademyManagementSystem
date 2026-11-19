namespace FAMS.Application.Content.Common.Dtos;

public sealed record GalleryImageDto(
    Guid Id,
    string ImageUrl,
    string? Caption,
    int SortOrder);