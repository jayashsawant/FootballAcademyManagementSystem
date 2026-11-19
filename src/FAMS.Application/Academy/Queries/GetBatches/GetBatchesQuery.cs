using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetBatches;

public sealed record GetBatchesQuery(
    Guid? BranchId,
    Guid? SeasonId,
    Guid? AgeGroupId,
    bool IncludeInactive = false) : IRequest<Result<IReadOnlyList<BatchDto>>>;