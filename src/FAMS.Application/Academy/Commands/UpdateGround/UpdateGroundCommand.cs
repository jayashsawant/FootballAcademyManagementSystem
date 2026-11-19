using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateGround;

public sealed record UpdateGroundCommand(
    Guid BranchId,
    Guid GroundId,
    string Name,
    string? Description) : IRequest<Result>;