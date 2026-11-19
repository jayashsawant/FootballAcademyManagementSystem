using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.Training.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Training;
using MediatR;

namespace FAMS.Application.Training.Commands.GenerateRecurringSessions;

public class GenerateRecurringSessionsCommandHandler
    : IRequestHandler<GenerateRecurringSessionsCommand, Result<GenerateRecurringSessionsResult>>
{
    private readonly ITrainingSessionRepository _sessionRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateRecurringSessionsCommandHandler(
        ITrainingSessionRepository sessionRepository,
        IBranchRepository branchRepository,
        ICoachRepository coachRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _branchRepository = branchRepository;
        _coachRepository = coachRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GenerateRecurringSessionsResult>> Handle(
        GenerateRecurringSessionsCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<GenerateRecurringSessionsResult>.Failure(
                Error.NotFound("TrainingSession.BranchNotFound", "The specified branch could not be found."));
        }

        var groundExists = branch.Grounds.Any(g => g.Id == request.GroundId);

        if (!groundExists)
        {
            return Result<GenerateRecurringSessionsResult>.Failure(
                Error.NotFound("TrainingSession.GroundNotFound", "The specified ground could not be found on this branch."));
        }

        var coach = await _coachRepository.GetByIdAsync(request.CoachId, cancellationToken);

        if (coach is null)
        {
            return Result<GenerateRecurringSessionsResult>.Failure(
                Error.NotFound("TrainingSession.CoachNotFound", "The specified coach could not be found."));
        }

        var daysOfWeekSet = request.DaysOfWeek.ToHashSet();
        var sessions = new List<TrainingSession>();

        for (var date = request.StartDate.Date; date <= request.EndDate.Date; date = date.AddDays(1))
        {
            if (!daysOfWeekSet.Contains(date.DayOfWeek))
                continue;

            var session = TrainingSession.Create(
                request.BranchId,
                request.GroundId,
                request.SeasonId,
                request.CoachId,
                date,
                request.StartTime,
                request.EndTime,
                request.Notes);

            sessions.Add(session);
        }

        if (sessions.Count == 0)
        {
            return Result<GenerateRecurringSessionsResult>.Failure(
                Error.Validation("TrainingSession.NoMatchingDates", "No dates in the specified range matched the selected days of the week."));
        }

        await _sessionRepository.AddRangeAsync(sessions, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new GenerateRecurringSessionsResult(sessions.Count);
    }
}