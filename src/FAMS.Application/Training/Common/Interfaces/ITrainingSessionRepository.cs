using FAMS.Domain.Training;

namespace FAMS.Application.Training.Common.Interfaces;

public interface ITrainingSessionRepository
{
    Task<TrainingSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<TrainingSession> Sessions, int TotalCount)> SearchAsync(
        Guid? branchId,
        Guid? coachId,
        Guid? seasonId,
        DateTime? fromDate,
        DateTime? toDate,
        TrainingSessionStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(TrainingSession session, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TrainingSession> sessions, CancellationToken cancellationToken = default);

    Task AddAttendanceRecordAsync(AttendanceRecord record, CancellationToken cancellationToken = default);
}