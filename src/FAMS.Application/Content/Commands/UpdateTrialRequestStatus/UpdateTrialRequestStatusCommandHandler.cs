using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FAMS.Application.Content.Commands.UpdateTrialRequestStatus;

public class UpdateTrialRequestStatusCommandHandler : IRequestHandler<UpdateTrialRequestStatusCommand, Result>
{
	private readonly ITrialRequestRepository _trialRequestRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateTrialRequestStatusCommandHandler(ITrialRequestRepository trialRequestRepository, IUnitOfWork unitOfWork)
	{
		_trialRequestRepository = trialRequestRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(UpdateTrialRequestStatusCommand request, CancellationToken cancellationToken)
	{
		var trialRequest = await _trialRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);

		if (trialRequest is null)
		{
			return Result.Failure(Error.NotFound("TrialRequest.NotFound", "The specified trial request could not be found."));
		}

		trialRequest.UpdateStatus(request.Status, request.Notes);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}