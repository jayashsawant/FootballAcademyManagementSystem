using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.TransferCoachToBranch;

public sealed record TransferCoachToBranchCommand(Guid CoachId, Guid NewBranchId) : IRequest<Result>;