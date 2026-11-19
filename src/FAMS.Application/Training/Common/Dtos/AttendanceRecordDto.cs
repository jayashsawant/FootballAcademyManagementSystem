namespace FAMS.Application.Training.Common.Dtos;

public sealed record AttendanceRecordDto(
    Guid Id,
    Guid PlayerId,
    string PlayerName,
    string Status,
    DateTime MarkedAt);