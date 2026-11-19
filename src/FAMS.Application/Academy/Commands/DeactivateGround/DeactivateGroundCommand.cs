using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateGround;

public sealed record DeactivateGroundCommand(Guid BranchId, Guid GroundId) : IRequest<Result>;