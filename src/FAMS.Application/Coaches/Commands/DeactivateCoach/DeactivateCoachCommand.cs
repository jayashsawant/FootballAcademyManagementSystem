using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.DeactivateCoach;

public sealed record DeactivateCoachCommand(Guid CoachId) : IRequest<Result>;