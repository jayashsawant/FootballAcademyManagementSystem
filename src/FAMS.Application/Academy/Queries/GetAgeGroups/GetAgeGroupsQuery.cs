using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetAgeGroups;

public sealed record GetAgeGroupsQuery(
    Guid AcademyId,
    bool IncludeInactive = false
) : IRequest<Result<IReadOnlyList<AgeGroupDto>>>;