using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateBatch;

public sealed record ReactivateBatchCommand(Guid BatchId) : IRequest<Result>;