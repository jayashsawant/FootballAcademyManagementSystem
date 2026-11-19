using FAMS.Domain.Common;

namespace FAMS.Domain.People;

public class PlayerEnrollment : AuditableEntity
{
    public Guid PlayerId { get; private set; }

    public Guid SeasonId { get; private set; }

    public Guid BranchId { get; private set; }

    public Guid AgeGroupId { get; private set; }

    public Guid BatchId { get; private set; }

    public Guid? TeamId { get; private set; }

    public DateTime EnrollmentDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public bool IsCurrent { get; private set; }

    public PlayerEnrollmentStatus Status { get; private set; }

    private PlayerEnrollment(
        Guid playerId,
        Guid seasonId,
        Guid branchId,
        Guid ageGroupId,
        Guid batchId,
        Guid? teamId,
        DateTime enrollmentDate)
    {
        PlayerId = playerId;
        SeasonId = seasonId;
        BranchId = branchId;
        AgeGroupId = ageGroupId;
        BatchId = batchId;
        TeamId = teamId;
        EnrollmentDate = enrollmentDate;
        IsCurrent = true;
        Status = PlayerEnrollmentStatus.Active;
    }

    public static PlayerEnrollment Create(
        Guid playerId,
        Guid seasonId,
        Guid branchId,
        Guid ageGroupId,
        Guid batchId,
        Guid? teamId = null,
        DateTime? enrollmentDate = null)
    {
        if (playerId == Guid.Empty)
            throw new ArgumentException(
                "Player is required.",
                nameof(playerId));

        if (seasonId == Guid.Empty)
            throw new ArgumentException(
                "Season is required.",
                nameof(seasonId));

        if (branchId == Guid.Empty)
            throw new ArgumentException(
                "Branch is required.",
                nameof(branchId));

        if (ageGroupId == Guid.Empty)
            throw new ArgumentException(
                "Age group is required.",
                nameof(ageGroupId));

        if (batchId == Guid.Empty)
            throw new ArgumentException(
                "Batch is required.",
                nameof(batchId));

        if (teamId == Guid.Empty)
            teamId = null;

        return new PlayerEnrollment(
            playerId,
            seasonId,
            branchId,
            ageGroupId,
            batchId,
            teamId,
            enrollmentDate?.Date ?? DateTime.UtcNow.Date);
    }

    public void ChangeBatch(Guid batchId)
    {
        if (batchId == Guid.Empty)
            throw new ArgumentException(
                "Batch is required.",
                nameof(batchId));

        BatchId = batchId;
    }

    public void ChangeTeam(Guid? teamId)
    {
        if (teamId == Guid.Empty)
            teamId = null;

        TeamId = teamId;
    }

    public void TransferToBranch(Guid branchId)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException(
                "Branch is required.",
                nameof(branchId));

        BranchId = branchId;
    }

    public void ChangeAgeGroup(Guid ageGroupId)
    {
        if (ageGroupId == Guid.Empty)
            throw new ArgumentException(
                "Age group is required.",
                nameof(ageGroupId));

        AgeGroupId = ageGroupId;
    }

    public void MarkAsCompleted(DateTime? endDate = null)
    {
        Status = PlayerEnrollmentStatus.Completed;
        EndDate = endDate?.Date ?? DateTime.UtcNow.Date;
        IsCurrent = false;
    }

    public void MarkAsTransferred(DateTime? endDate = null)
    {
        Status = PlayerEnrollmentStatus.Transferred;
        EndDate = endDate?.Date ?? DateTime.UtcNow.Date;
        IsCurrent = false;
    }

    public void MarkAsWithdrawn(DateTime? endDate = null)
    {
        Status = PlayerEnrollmentStatus.Withdrawn;
        EndDate = endDate?.Date ?? DateTime.UtcNow.Date;
        IsCurrent = false;
    }

    public void SetAsCurrent()
    {
        Status = PlayerEnrollmentStatus.Active;
        EndDate = null;
        IsCurrent = true;
    }
}