using FAMS.Application.Training.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Training.Commands.CreateTrainingSession;

public sealed record CreateTrainingSessionCommand(
    Guid BranchId,
    Guid GroundId,
    Guid SeasonId,
    Guid CoachId,
    DateTime SessionDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? Notes) : IRequest<Result<TrainingSessionDto>>;