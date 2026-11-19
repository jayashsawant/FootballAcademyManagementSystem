using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.DeactivateCoach;

public class DeactivateCoachCommandHandler : IRequestHandler<DeactivateCoachCommand, Result>
{
    private readonly ICoachRepository _coachRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateCoachCommandHandler(
        ICoachRepository coachRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _coachRepository = coachRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateCoachCommand request, CancellationToken cancellationToken)
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
                Error.Conflict("Coach.AlreadyTerminated", "This coach is already terminated."));
        }

        coach.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}