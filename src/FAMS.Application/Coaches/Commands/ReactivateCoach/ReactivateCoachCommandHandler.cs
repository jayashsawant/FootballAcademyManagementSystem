using FAMS.Application.Coaches.Commands.DeactivateCoach;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.ReactivateCoach;

public class ReactivateCoachCommandHandler : IRequestHandler<ReactivateCoachCommand, Result>
{
    private readonly ICoachRepository _coachRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateCoachCommandHandler(ICoachRepository coachRepository, IUnitOfWork unitOfWork)
    {
        _coachRepository = coachRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateCoachCommand request, CancellationToken cancellationToken)
    {
        // Coach is soft-deleted on Deactivate (Status = Terminated + IsDeleted = true),
        // so this lookup must bypass the global query filter — same pattern as ReactivateBranch/ReactivateUser.
        var coach = await _coachRepository.GetByIdIncludingDeletedAsync(request.CoachId, cancellationToken);

        if (coach is null)
        {
            return Result.Failure(
                Error.NotFound("Coach.NotFound", "The specified coach could not be found."));
        }

        if (coach.Status != Domain.Coaches.CoachStatus.Terminated)
        {
            return Result.Failure(
                Error.Conflict("Coach.NotTerminated", "This coach is not currently terminated."));
        }

        coach.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}