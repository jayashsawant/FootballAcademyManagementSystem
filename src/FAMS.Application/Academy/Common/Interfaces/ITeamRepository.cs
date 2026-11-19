using FAMS.Domain.Academy;

namespace FAMS.Application.Academy.Common.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Team?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Team>> GetAllAsync(
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
        Guid? excludeTeamId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Team team,
        CancellationToken cancellationToken = default);
}