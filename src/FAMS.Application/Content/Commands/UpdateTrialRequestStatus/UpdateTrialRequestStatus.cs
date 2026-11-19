using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateTrialRequestStatus;

public sealed record UpdateTrialRequestStatusCommand(Guid RequestId, TrialRequestStatus Status, string? Notes) : IRequest<Result>;