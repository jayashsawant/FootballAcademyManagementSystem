using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Models;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Application.Training.Common.Dtos;
using FAMS.Application.Training.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Training.Queries.GetTrainingSessions;

public class GetTrainingSessionsQueryHandler : IRequestHandler<GetTrainingSessionsQuery, Result<PagedResult<TrainingSessionDto>>>
{
    private readonly ITrainingSessionRepository _sessionRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetTrainingSessionsQueryHandler(
        ITrainingSessionRepository sessionRepository,
        IBranchRepository branchRepository,
        ICoachRepository coachRepository,
        IPlayerRepository playerRepository)
    {
        _sessionRepository = sessionRepository;
        _branchRepository = branchRepository;
        _coachRepository = coachRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Result<PagedResult<TrainingSessionDto>>> Handle(GetTrainingSessionsQuery request, CancellationToken cancellationToken)
    {
        var (sessions, totalCount) = await _sessionRepository.SearchAsync(
            request.BranchId,
            request.CoachId,
            request.SeasonId,
            request.FromDate,
            request.ToDate,
            request.Status,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        // Small reference sets — load once and join in memory so the list avoids N+1.
        var branches = await _branchRepository.GetAllAsync(includeInactive: true, cancellationToken);
        var coaches = await _coachRepository.GetAllAsync(cancellationToken);
        var players = await _playerRepository.GetAllAsync(cancellationToken);

        var branchById = branches.ToDictionary(b => b.Id);
        var coachById = coaches.ToDictionary(c => c.Id);
        var playerNameById = players.ToDictionary(p => p.Id, p => $"{p.FirstName} {p.LastName}");

        var sessionDtos = sessions
            .Select(session =>
            {
                branchById.TryGetValue(session.BranchId, out var branch);
                coachById.TryGetValue(session.CoachId, out var coach);

                var groundName = branch?.Grounds.FirstOrDefault(g => g.Id == session.GroundId)?.Name ?? string.Empty;

                var attendance = session.AttendanceRecords
                    .Select(a => new AttendanceRecordDto(
                        a.Id,
                        a.PlayerId,
                        playerNameById.TryGetValue(a.PlayerId, out var name) ? name : "Unknown player",
                        a.Status.ToString(),
                        a.MarkedAt))
                    .ToList();

                return new TrainingSessionDto(
                    session.Id,
                    session.BranchId,
                    branch?.Name ?? string.Empty,
                    session.GroundId,
                    groundName,
                    session.SeasonId,
                    session.CoachId,
                    coach is not null ? $"{coach.FirstName} {coach.LastName}" : string.Empty,
                    session.SessionDate,
                    session.StartTime,
                    session.EndTime,
                    session.Status.ToString(),
                    session.Notes,
                    attendance);
            })
            .ToList();

        return new PagedResult<TrainingSessionDto>(sessionDtos, request.PageNumber, request.PageSize, totalCount);
    }
}
