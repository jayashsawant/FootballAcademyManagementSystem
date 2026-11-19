using FAMS.Application.Common.Models;
using FAMS.Application.Coaches.Common.Dtos;
using FAMS.Domain.Coaches;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Queries.GetCoaches;

public sealed record GetCoachesQuery(
    string? SearchTerm,
    Guid? BranchId,
    CoachStatus? Status,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<CoachDto>>>;