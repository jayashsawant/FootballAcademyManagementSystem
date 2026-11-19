namespace FAMS.Application.Training.Common.Dtos;

public sealed record TrainingSessionDto(
    Guid Id,
    Guid BranchId,
    string BranchName,
    Guid GroundId,
    string GroundName,
    Guid SeasonId,
    Guid CoachId,
    string CoachName,
    DateTime SessionDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Status,
    string? Notes,
    IReadOnlyList<AttendanceRecordDto> AttendanceRecords);