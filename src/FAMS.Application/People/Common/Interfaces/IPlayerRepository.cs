using FAMS.Domain.People;

namespace FAMS.Application.People.Common.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Player?> GetByIdIncludingDeletedAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Player>> GetActiveAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Player>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Player> Players, int TotalCount)> SearchAsync(
        string? searchTerm,
        Guid? branchId,
        Guid? seasonId,
        Guid? ageGroupId,
        Guid? batchId,
        Guid? teamId,
        Guid? parentId,
        PlayerStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Player player,
        CancellationToken cancellationToken = default);
}