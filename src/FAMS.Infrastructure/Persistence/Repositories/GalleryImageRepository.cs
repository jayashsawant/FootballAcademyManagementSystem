using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class GalleryImageRepository : IGalleryImageRepository
{
    private readonly ApplicationDbContext _context;

    public GalleryImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<GalleryImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.GalleryImages.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GalleryImage>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.GalleryImages.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(GalleryImage image, CancellationToken cancellationToken = default)
    {
        await _context.GalleryImages.AddAsync(image, cancellationToken);
    }

    public Task RemoveAsync(GalleryImage image, CancellationToken cancellationToken = default)
    {
        _context.GalleryImages.Remove(image);
        return Task.CompletedTask;
    }
}