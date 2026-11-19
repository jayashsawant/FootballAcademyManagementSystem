using FAMS.Application.Common.Models;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetTrialRequests;

public class GetTrialRequestsQueryHandler : IRequestHandler<GetTrialRequestsQuery, Result<PagedResult<TrialRequestDto>>>
{
    private readonly ITrialRequestRepository _trialRequestRepository;

    public GetTrialRequestsQueryHandler(ITrialRequestRepository trialRequestRepository)
    {
        _trialRequestRepository = trialRequestRepository;
    }

    public async Task<Result<PagedResult<TrialRequestDto>>> Handle(GetTrialRequestsQuery request, CancellationToken cancellationToken)
    {
        var (requests, totalCount) = await _trialRequestRepository.SearchAsync(request.Status, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = requests
            .Select(r => new TrialRequestDto(r.Id, r.Name, r.DateOfBirth, r.Email, r.Mobile, r.Status.ToString(), r.Notes, r.CreatedAt))
            .ToList();

        return new PagedResult<TrialRequestDto>(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}