using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateBatch;

public sealed record DeactivateBatchCommand(Guid BatchId) : IRequest<Result>;