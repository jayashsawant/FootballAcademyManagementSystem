using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Coaches.Common.Dtos;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Coaches;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.CreateCoach;

public class CreateCoachCommandHandler : IRequestHandler<CreateCoachCommand, Result<CoachDto>>
{
    private const string CoachPrefix = "COA";

    private readonly ICoachRepository _coachRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IBusinessIdGenerator _businessIdGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCoachCommandHandler(
        ICoachRepository coachRepository,
        IBranchRepository branchRepository,
        IBusinessIdGenerator businessIdGenerator,
        IUnitOfWork unitOfWork)
    {
        _coachRepository = coachRepository;
        _branchRepository = branchRepository;
        _businessIdGenerator = businessIdGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CoachDto>> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<CoachDto>.Failure(
                Error.NotFound("Coach.BranchNotFound", "The specified branch could not be found."));
        }

        var phoneExists = await _coachRepository.PhoneExistsAsync(request.Phone, cancellationToken: cancellationToken);

        if (phoneExists)
        {
            return Result<CoachDto>.Failure(
                Error.Conflict("Coach.PhoneInUse", "A coach with this phone number already exists."));
        }

        var coachNumber = await _businessIdGenerator.GenerateAsync(CoachPrefix, cancellationToken);

        var employmentType = (EmploymentType)request.EmploymentType;

        var coach = Coach.Create(
            coachNumber,
            request.BranchId,
            request.FirstName,
            request.LastName,
            request.Phone,
            employmentType,
            request.Email,
            request.Qualification,
            request.LicenseLevel,
            request.YearsExperience);

        await _coachRepository.AddAsync(coach, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CoachDto(
            coach.Id,
            coach.CoachNumber,
            coach.BranchId,
            branch.Name,
            coach.FirstName,
            coach.LastName,
            coach.Email,
            coach.Phone,
            coach.Qualification,
            coach.LicenseLevel,
            coach.YearsExperience,
            coach.EmploymentType.ToString(),
            coach.Status.ToString(),
            coach.IsDeleted,
            coach.CreatedAt);
    }
}