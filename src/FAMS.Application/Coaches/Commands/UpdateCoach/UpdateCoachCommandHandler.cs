using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Coaches;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.UpdateCoach;

public class UpdateCoachCommandHandler : IRequestHandler<UpdateCoachCommand, Result>
{
    private readonly ICoachRepository _coachRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCoachCommandHandler(ICoachRepository coachRepository, IUnitOfWork unitOfWork)
    {
        _coachRepository = coachRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await _coachRepository.GetByIdAsync(request.CoachId, cancellationToken);

        if (coach is null)
        {
            return Result.Failure(
                Error.NotFound("Coach.NotFound", "The specified coach could not be found."));
        }

        if (coach.Status == CoachStatus.Terminated)
        {
            return Result.Failure(
                Error.Conflict("Coach.IsTerminated", "This coach has been terminated and cannot be edited. Reactivate them first."));
        }

        var phoneChanging = !string.Equals(request.Phone, coach.Phone, StringComparison.Ordinal);

        if (phoneChanging)
        {
            var phoneExists = await _coachRepository.PhoneExistsAsync(request.Phone, request.CoachId, cancellationToken);

            if (phoneExists)
            {
                return Result.Failure(
                    Error.Conflict("Coach.PhoneInUse", "A coach with this phone number already exists."));
            }
        }

        coach.UpdateDetails(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.Qualification,
            request.LicenseLevel,
            request.YearsExperience);

        var employmentType = (EmploymentType)request.EmploymentType;
        coach.ChangeEmploymentType(employmentType);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}