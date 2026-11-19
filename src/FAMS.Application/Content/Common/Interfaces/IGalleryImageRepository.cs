using FAMS.Domain.Content;

namespace FAMS.Application.Content.Common.Interfaces;

public interface IGalleryImageRepository
{
    Task<GalleryImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GalleryImage>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(GalleryImage image, CancellationToken cancellationToken = default);
    Task RemoveAsync(GalleryImage image, CancellationToken cancellationToken = default);
}