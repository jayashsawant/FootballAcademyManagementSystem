using FAMS.Application.Common.Models;
using FAMS.Application.Identity.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetUsers;

public sealed record GetUsersQuery(
    string? SearchTerm,
    Guid? RoleId,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<UserDto>>>;