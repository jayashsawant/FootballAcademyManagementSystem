using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateBranch;

public sealed record UpdateBranchCommand(
    Guid BranchId,
    string Name,
    string? Address,
    string? City,
    string? ContactPhone,
    string? ContactEmail) : IRequest<Result>;