using FAMS.Domain.Academy;

namespace FAMS.Application.Academy.Common.Interfaces;

public interface IBatchRepository
{
    Task<Batch?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Batch?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Batch>> GetAllAsync(
        Guid? branchId = null,
        Guid? seasonId = null,
        Guid? ageGroupId = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(
        Guid academyId,
        Guid branchId,
        Guid seasonId,
        Guid ageGroupId,
        string name,
        Guid? excludeBatchId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Batch batch,
        CancellationToken cancellationToken = default);
}