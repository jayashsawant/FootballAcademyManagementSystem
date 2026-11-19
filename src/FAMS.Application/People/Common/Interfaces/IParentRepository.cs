using FAMS.Domain.People;

namespace FAMS.Application.People.Common.Interfaces;

public interface IParentRepository
{
    Task<Parent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Parent?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> PhoneExistsAsync(string phone, Guid? excludeParentId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Parent> Parents, int TotalCount)> SearchAsync(
        string? searchTerm,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(Parent parent, CancellationToken cancellationToken = default);
}
