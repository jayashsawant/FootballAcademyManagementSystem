using FAMS.Domain.Academy;

namespace FAMS.Application.Academy.Common.Interfaces;

public interface IAgeGroupRepository
{
    Task<AgeGroup?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AgeGroup?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AgeGroup>> GetAllAsync(
        Guid academyId,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(
        Guid academyId,
        string name,
        Guid? excludeAgeGroupId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        AgeGroup ageGroup,
        CancellationToken cancellationToken = default);
}