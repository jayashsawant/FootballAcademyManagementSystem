using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateBranch;

public sealed record CreateBranchCommand(
    string Name,
    string? Address,
    string? City,
    string? ContactPhone,
    string? ContactEmail) : IRequest<Result<Guid>>;