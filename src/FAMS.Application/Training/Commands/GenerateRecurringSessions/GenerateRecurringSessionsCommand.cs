using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Training.Commands.GenerateRecurringSessions;

public sealed record GenerateRecurringSessionsCommand(
    Guid BranchId,
    Guid GroundId,
    Guid SeasonId,
    Guid CoachId,
    IReadOnlyList<DayOfWeek> DaysOfWeek,
    DateTime StartDate,
    DateTime EndDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? Notes) : IRequest<Result<GenerateRecurringSessionsResult>>;

public sealed record GenerateRecurringSessionsResult(int SessionsCreated);