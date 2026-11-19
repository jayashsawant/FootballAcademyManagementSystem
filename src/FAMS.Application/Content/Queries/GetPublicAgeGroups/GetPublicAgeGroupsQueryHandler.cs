using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicAgeGroups;

public class GetPublicAgeGroupsQueryHandler : IRequestHandler<GetPublicAgeGroupsQuery, Result<IReadOnlyList<AgeGroupDto>>>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IAcademyRepository _academyRepository;

    public GetPublicAgeGroupsQueryHandler(IAgeGroupRepository ageGroupRepository, IAcademyRepository academyRepository)
    {
        _ageGroupRepository = ageGroupRepository;
        _academyRepository = academyRepository;
    }

    public async Task<Result<IReadOnlyList<AgeGroupDto>>> Handle(GetPublicAgeGroupsQuery request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result<IReadOnlyList<AgeGroupDto>>.Failure(Error.NotFound("Academy.NotFound", "Academy is not configured."));
        }

        var ageGroups = await _ageGroupRepository.GetAllAsync(academy.Id, includeInactive: false, cancellationToken);

        var dtos = ageGroups
            .Select(a => new AgeGroupDto(a.Id, a.AcademyId, a.Name, a.MinimumAge, a.MaximumAge, a.SortOrder, a.IsActive))
            .ToList();

        return dtos;
    }
}