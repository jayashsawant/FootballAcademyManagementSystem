using FAMS.Application.Common.Models;
using FAMS.Application.People.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetParents;

public sealed record GetParentsQuery(
    string? SearchTerm,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<ParentDto>>>;
