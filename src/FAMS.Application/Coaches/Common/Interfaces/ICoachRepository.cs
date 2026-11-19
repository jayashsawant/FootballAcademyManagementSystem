using FAMS.Domain.Coaches;

namespace FAMS.Application.Coaches.Common.Interfaces;

public interface ICoachRepository
{
    Task<Coach?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Coach?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> PhoneExistsAsync(string phone, Guid? excludeCoachId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Coach> Coaches, int TotalCount)> SearchAsync(
        string? searchTerm,
        Guid? branchId,
        CoachStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Coach>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Coach coach, CancellationToken cancellationToken = default);
}
