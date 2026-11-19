using FAMS.Domain.Content;

namespace FAMS.Application.Content.Common.Interfaces;

public interface ITrialRequestRepository
{
    Task<TrialRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<TrialRequest> Requests, int TotalCount)> SearchAsync(
        TrialRequestStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(TrialRequest request, CancellationToken cancellationToken = default);
}