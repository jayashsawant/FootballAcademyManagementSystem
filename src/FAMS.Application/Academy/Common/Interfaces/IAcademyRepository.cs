using FAMS.Domain.Academy;

namespace FAMS.Application.Academy.Common.Interfaces;

public interface IAcademyRepository
{
    Task<Domain.Academy.Academy?> GetAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Domain.Academy.Academy academy, CancellationToken cancellationToken = default);
}