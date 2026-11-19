using FAMS.Domain.Common;
using FAMS.Domain.Training;
using MediatR;

namespace FAMS.Application.Training.Commands.MarkAttendance;

public sealed record MarkAttendanceCommand(
    Guid TrainingSessionId,
    Guid PlayerId,
    AttendanceStatus Status) : IRequest<Result<Guid>>;