using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateBranch;

public sealed record DeactivateBranchCommand(Guid BranchId) : IRequest<Result>;