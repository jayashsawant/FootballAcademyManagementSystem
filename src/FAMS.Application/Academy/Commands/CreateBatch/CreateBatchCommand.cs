using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateBatch;

public sealed record CreateBatchCommand(
    Guid BranchId,
    Guid SeasonId,
    Guid AgeGroupId,
    string Name) : IRequest<Result<BatchDto>>;