using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetAgeGroups;

public sealed class GetAgeGroupsQueryHandler
    : IRequestHandler<
        GetAgeGroupsQuery,
        Result<IReadOnlyList<AgeGroupDto>>>
{
    private readonly IAgeGroupRepository _ageGroupRepository;

    public GetAgeGroupsQueryHandler(
        IAgeGroupRepository ageGroupRepository)
    {
        _ageGroupRepository = ageGroupRepository;
    }

    public async Task<Result<IReadOnlyList<AgeGroupDto>>> Handle(
        GetAgeGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var ageGroups =
            await _ageGroupRepository.GetAllAsync(
                request.AcademyId,
                request.IncludeInactive,
                cancellationToken);

        var dtos = ageGroups
            .Select(a =>
                new AgeGroupDto(
                    a.Id,
                    a.AcademyId,
                    a.Name,
                    a.MinimumAge,
                    a.MaximumAge,
                    a.SortOrder,
                    a.IsActive))
            .ToList();

        return dtos;
    }
}