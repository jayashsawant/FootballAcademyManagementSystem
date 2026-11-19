using FAMS.Domain.Academy;

namespace FAMS.Application.Academy.Common.Interfaces;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Branch>> GetAllAsync(bool includeInactive = false, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(Guid academyId, string name, Guid? excludeBranchId = null, CancellationToken cancellationToken = default);

    Task AddAsync(Branch branch, CancellationToken cancellationToken = default);
    Task AddGroundAsync(Ground ground, CancellationToken cancellationToken = default);

    Task<Branch?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default);
}