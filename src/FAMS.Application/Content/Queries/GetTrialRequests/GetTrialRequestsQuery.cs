using FAMS.Application.Common.Models;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Queries.GetTrialRequests;

public sealed record GetTrialRequestsQuery(
    TrialRequestStatus? Status,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<TrialRequestDto>>>;