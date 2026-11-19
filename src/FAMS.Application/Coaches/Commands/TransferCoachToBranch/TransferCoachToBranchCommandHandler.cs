using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.TransferCoachToBranch;

public class TransferCoachToBranchCommandHandler : IRequestHandler<TransferCoachToBranchCommand, Result>
{
    private readonly ICoachRepository _coachRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferCoachToBranchCommandHandler(
        ICoachRepository coachRepository,
        IBranchRepository branchRepository,
        IUnitOfWork unitOfWork)
    {
        _coachRepository = coachRepository;
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(TransferCoachToBranchCommand request, CancellationToken cancellationToken)
    {
        var coach = await _coachRepository.GetByIdAsync(request.CoachId, cancellationToken);

        if (coach is null)
        {
            return Result.Failure(
                Error.NotFound("Coach.NotFound", "The specified coach could not be found."));
        }

        if (coach.Status == Domain.Coaches.CoachStatus.Terminated)
        {
            return Result.Failure(
                Error.Conflict("Coach.IsTerminated", "This coach has been terminated and cannot be transferred. Reactivate them first."));
        }

        var newBranch = await _branchRepository.GetByIdAsync(request.NewBranchId, cancellationToken);

        if (newBranch is null)
        {
            return Result.Failure(
                Error.NotFound("Coach.BranchNotFound", "The specified branch could not be found."));
        }

        if (coach.BranchId == request.NewBranchId)
        {
            return Result.Failure(
                Error.Conflict("Coach.AlreadyAtBranch", "This coach is already assigned to the specified branch."));
        }

        coach.TransferToBranch(request.NewBranchId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}