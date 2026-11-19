using FAMS.Application.Common.Models;
using FAMS.Application.People.Common.Dtos;
using FAMS.Domain.Common;
using FAMS.Domain.People;
using MediatR;

namespace FAMS.Application.People.Queries.GetPlayers;

public sealed record GetPlayersQuery(
    string? SearchTerm,
    Guid? BranchId,
    Guid? SeasonId,
    Guid? AgeGroupId,
    Guid? BatchId,
    Guid? TeamId,
    Guid? ParentId,
    PlayerStatus? Status,
    int PageNumber = 1,
    int PageSize = 20)
    : IRequest<Result<PagedResult<PlayerDto>>>;