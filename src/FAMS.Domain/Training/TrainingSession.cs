using FAMS.Domain.Common;

namespace FAMS.Domain.Training;

public class TrainingSession : AuditableEntity, IAggregateRoot
{
    private readonly List<AttendanceRecord> _attendanceRecords = new();

    public Guid BranchId { get; private set; }
    public Guid GroundId { get; private set; }
    public Guid SeasonId { get; private set; }
    public Guid CoachId { get; private set; }
    public DateTime SessionDate { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public TrainingSessionStatus Status { get; private set; }
    public string? Notes { get; private set; }

    public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

    private TrainingSession(
        Guid branchId,
        Guid groundId,
        Guid seasonId,
        Guid coachId,
        DateTime sessionDate,
        TimeSpan startTime,
        TimeSpan endTime,
        string? notes)
    {
        BranchId = branchId;
        GroundId = groundId;
        SeasonId = seasonId;
        CoachId = coachId;
        SessionDate = sessionDate;
        StartTime = startTime;
        EndTime = endTime;
        Notes = notes;
        Status = TrainingSessionStatus.Scheduled;
    }

    public static TrainingSession Create(
        Guid branchId,
        Guid groundId,
        Guid seasonId,
        Guid coachId,
        DateTime sessionDate,
        TimeSpan startTime,
        TimeSpan endTime,
        string? notes = null)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException("Branch is required.", nameof(branchId));

        if (groundId == Guid.Empty)
            throw new ArgumentException("Ground is required.", nameof(groundId));

        if (seasonId == Guid.Empty)
            throw new ArgumentException("Season is required.", nameof(seasonId));

        if (coachId == Guid.Empty)
            throw new ArgumentException("Coach is required.", nameof(coachId));

        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.", nameof(endTime));

        return new TrainingSession(branchId, groundId, seasonId, coachId, sessionDate.Date, startTime, endTime, notes?.Trim());
    }

    public void Reschedule(DateTime sessionDate, TimeSpan startTime, TimeSpan endTime)
    {
        if (Status == TrainingSessionStatus.Cancelled)
            throw new InvalidOperationException("Cannot reschedule a cancelled session.");

        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.", nameof(endTime));

        SessionDate = sessionDate.Date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void Cancel()
    {
        Status = TrainingSessionStatus.Cancelled;
    }

    public void Complete()
    {
        if (Status == TrainingSessionStatus.Cancelled)
            throw new InvalidOperationException("Cannot complete a cancelled session.");

        Status = TrainingSessionStatus.Completed;
    }

    public AttendanceRecord MarkAttendance(Guid playerId, AttendanceStatus status, Guid? markedBy = null)
    {
        var existing = _attendanceRecords.FirstOrDefault(a => a.PlayerId == playerId);

        if (existing is not null)
        {
            existing.UpdateStatus(status, markedBy);
            return existing;
        }

        var record = AttendanceRecord.Create(Id, playerId, status, markedBy);
        _attendanceRecords.Add(record);
        return record;
    }
}