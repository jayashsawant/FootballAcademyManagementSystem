using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.AddGround;

public sealed record AddGroundCommand(
    Guid BranchId,
    string Name,
    string? Description) : IRequest<Result<Guid>>;