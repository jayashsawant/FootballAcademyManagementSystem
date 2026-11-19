using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.ReactivateCoach;

public sealed record ReactivateCoachCommand(Guid CoachId) : IRequest<Result>;