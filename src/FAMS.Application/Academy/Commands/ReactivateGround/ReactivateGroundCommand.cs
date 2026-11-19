using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateGround;

public sealed record ReactivateGroundCommand(Guid BranchId, Guid GroundId) : IRequest<Result>;