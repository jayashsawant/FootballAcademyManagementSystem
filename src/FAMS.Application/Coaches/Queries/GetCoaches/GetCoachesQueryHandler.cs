using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Models;
using FAMS.Application.Coaches.Common.Dtos;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Queries.GetCoaches;

public class GetCoachesQueryHandler : IRequestHandler<GetCoachesQuery, Result<PagedResult<CoachDto>>>
{
    private readonly ICoachRepository _coachRepository;
    private readonly IBranchRepository _branchRepository;

    public GetCoachesQueryHandler(ICoachRepository coachRepository, IBranchRepository branchRepository)
    {
        _coachRepository = coachRepository;
        _branchRepository = branchRepository;
    }

    public async Task<Result<PagedResult<CoachDto>>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
    {
        var (coaches, totalCount) = await _coachRepository.SearchAsync(
            request.SearchTerm,
            request.BranchId,
            request.Status,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var coachDtos = new List<CoachDto>();

        foreach (var coach in coaches)
        {
            var branch = await _branchRepository.GetByIdAsync(coach.BranchId, cancellationToken);
            var branchName = branch?.Name ?? "Unknown";

            coachDtos.Add(new CoachDto(
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
                coach.CreatedAt));
        }

        var pagedResult = new PagedResult<CoachDto>(
            coachDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return pagedResult;
    }
}