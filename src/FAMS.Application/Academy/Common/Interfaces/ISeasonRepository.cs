using FAMS.Domain.Academy;

namespace FAMS.Application.Academy.Common.Interfaces;

public interface ISeasonRepository
{
    Task<Season?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Season?> GetCurrentAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Season season, CancellationToken cancellationToken = default);
}