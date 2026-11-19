using FAMS.Application.People.Common.Dtos;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetParentById;

public class GetParentByIdQueryHandler : IRequestHandler<GetParentByIdQuery, Result<ParentDto>>
{
    private readonly IParentRepository _parentRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetParentByIdQueryHandler(IParentRepository parentRepository, IPlayerRepository playerRepository)
    {
        _parentRepository = parentRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Result<ParentDto>> Handle(GetParentByIdQuery request, CancellationToken cancellationToken)
    {
        var parent = await _parentRepository.GetByIdAsync(request.ParentId, cancellationToken);

        if (parent is null)
        {
            return Result<ParentDto>.Failure(
                Error.NotFound("Parent.NotFound", "The specified parent could not be found."));
        }

        var children = await ChildrenForParentAsync(parent.Id, cancellationToken);

        return new ParentDto(
            parent.Id,
            parent.FirstName,
            parent.LastName,
            parent.Phone,
            parent.Email,
            parent.Address,
            parent.IsDeleted,
            parent.CreatedAt,
            children);
    }

    private async Task<IReadOnlyList<PlayerSummaryDto>> ChildrenForParentAsync(Guid parentId, CancellationToken cancellationToken)
    {
        var players = await _playerRepository.GetActiveAsync(cancellationToken);

        return players
            .Where(p => p.ParentId == parentId)
            .Select(p => new PlayerSummaryDto(p.Id, p.PlayerNumber, p.FirstName, p.LastName, p.Status.ToString()))
            .ToList();
    }
}
