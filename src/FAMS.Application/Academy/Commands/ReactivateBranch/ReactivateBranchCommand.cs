using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateBranch;

public sealed record ReactivateBranchCommand(Guid BranchId) : IRequest<Result>;