using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicBatches;

public sealed record GetPublicBatchesQuery(Guid? BranchId, Guid? AgeGroupId) : IRequest<Result<IReadOnlyList<BatchDto>>>;