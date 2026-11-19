using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Coaches.Common.Dtos;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Queries.GetCoachById;

public class GetCoachByIdQueryHandler : IRequestHandler<GetCoachByIdQuery, Result<CoachDto>>
{
    private readonly ICoachRepository _coachRepository;
    private readonly IBranchRepository _branchRepository;

    public GetCoachByIdQueryHandler(ICoachRepository coachRepository, IBranchRepository branchRepository)
    {
        _coachRepository = coachRepository;
        _branchRepository = branchRepository;
    }

    public async Task<Result<CoachDto>> Handle(GetCoachByIdQuery request, CancellationToken cancellationToken)
    {
        var coach = await _coachRepository.GetByIdAsync(request.CoachId, cancellationToken);

        if (coach is null)
        {
            return Result<CoachDto>.Failure(
                Error.NotFound("Coach.NotFound", "The specified coach could not be found."));
        }

        var branch = await _branchRepository.GetByIdAsync(coach.BranchId, cancellationToken);
        var branchName = branch?.Name ?? "Unknown";

        return new CoachDto(
            coach.Id,
            coach.CoachNumber,
            coach.BranchId,
            branchName,
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