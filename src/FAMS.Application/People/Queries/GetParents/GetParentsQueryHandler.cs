using FAMS.Application.Common.Models;
using FAMS.Application.People.Common.Dtos;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetParents;

public class GetParentsQueryHandler : IRequestHandler<GetParentsQuery, Result<PagedResult<ParentDto>>>
{
    private readonly IParentRepository _parentRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetParentsQueryHandler(IParentRepository parentRepository, IPlayerRepository playerRepository)
    {
        _parentRepository = parentRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Result<PagedResult<ParentDto>>> Handle(GetParentsQuery request, CancellationToken cancellationToken)
    {
        var (parents, totalCount) = await _parentRepository.SearchAsync(
            request.SearchTerm, request.IsActive, request.PageNumber, request.PageSize, cancellationToken);

        // Fetch the (small) set of active players once and group by parent so the
        // list can show each parent's linked children without N+1 queries.
        var playersByParent = (await _playerRepository.GetActiveAsync(cancellationToken))
            .GroupBy(p => p.ParentId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(p => new PlayerSummaryDto(p.Id, p.PlayerNumber, p.FirstName, p.LastName, p.Status.ToString())).ToList());

        var parentDtos = parents
            .Select(p => new ParentDto(
                p.Id,
                p.FirstName,
                p.LastName,
                p.Phone,
                p.Email,
                p.Address,
                p.IsDeleted,
                p.CreatedAt,
                playersByParent.TryGetValue(p.Id, out var children) ? children : new List<PlayerSummaryDto>()))
            .ToList();

        var pagedResult = new PagedResult<ParentDto>(parentDtos, request.PageNumber, request.PageSize, totalCount);

        return pagedResult;
    }
}
