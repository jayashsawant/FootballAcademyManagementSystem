using FAMS.Domain.Common;

namespace FAMS.Domain.Training;

public class AttendanceRecord : AuditableEntity
{
    public Guid TrainingSessionId { get; private set; }
    public Guid PlayerId { get; private set; }
    public AttendanceStatus Status { get; private set; }
    public DateTime MarkedAt { get; private set; }
    public Guid? MarkedBy { get; private set; }

    private AttendanceRecord(Guid trainingSessionId, Guid playerId, AttendanceStatus status, Guid? markedBy)
    {
        TrainingSessionId = trainingSessionId;
        PlayerId = playerId;
        Status = status;
        MarkedAt = DateTime.UtcNow;
        MarkedBy = markedBy;
    }

    public static AttendanceRecord Create(Guid trainingSessionId, Guid playerId, AttendanceStatus status, Guid? markedBy = null)
    {
        if (trainingSessionId == Guid.Empty)
            throw new ArgumentException("Training session is required.", nameof(trainingSessionId));

        if (playerId == Guid.Empty)
            throw new ArgumentException("Player is required.", nameof(playerId));

        return new AttendanceRecord(trainingSessionId, playerId, status, markedBy);
    }

    public void UpdateStatus(AttendanceStatus status, Guid? markedBy)
    {
        Status = status;
        MarkedAt = DateTime.UtcNow;
        MarkedBy = markedBy;
    }
}