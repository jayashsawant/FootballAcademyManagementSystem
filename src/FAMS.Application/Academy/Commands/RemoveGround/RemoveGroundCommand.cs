using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.RemoveGround;

public sealed record RemoveGroundCommand(Guid BranchId, Guid GroundId) : IRequest<Result>;