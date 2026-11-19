using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateAgeGroup;

public sealed record CreateAgeGroupCommand(
    Guid AcademyId,
    string Name,
    int? MinimumAge,
    int? MaximumAge,
    int SortOrder) : IRequest<Result<AgeGroupDto>>;