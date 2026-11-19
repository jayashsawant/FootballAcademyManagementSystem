using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicAgeGroups;

public sealed record GetPublicAgeGroupsQuery : IRequest<Result<IReadOnlyList<AgeGroupDto>>>;