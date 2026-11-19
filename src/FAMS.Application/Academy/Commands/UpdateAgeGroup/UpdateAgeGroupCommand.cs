using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateAgeGroup;

public sealed record UpdateAgeGroupCommand(
    Guid AgeGroupId,
    string Name,
    int? MinimumAge,
    int? MaximumAge,
    int SortOrder) : IRequest<Result>;