using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateBatch;

public sealed record UpdateBatchCommand(Guid BatchId, string Name) : IRequest<Result>;