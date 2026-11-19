using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.SubmitTrialRequest;

public class SubmitTrialRequestCommandHandler : IRequestHandler<SubmitTrialRequestCommand, Result<TrialRequestDto>>
{
    private readonly ITrialRequestRepository _trialRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitTrialRequestCommandHandler(ITrialRequestRepository trialRequestRepository, IUnitOfWork unitOfWork)
    {
        _trialRequestRepository = trialRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TrialRequestDto>> Handle(SubmitTrialRequestCommand request, CancellationToken cancellationToken)
    {
        var trialRequest = TrialRequest.Create(request.Name, request.DateOfBirth, request.Email, request.Mobile);

        await _trialRequestRepository.AddAsync(trialRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TrialRequestDto(
            trialRequest.Id, trialRequest.Name, trialRequest.DateOfBirth, trialRequest.Email,
            trialRequest.Mobile, trialRequest.Status.ToString(), trialRequest.Notes, trialRequest.CreatedAt);
    }
}