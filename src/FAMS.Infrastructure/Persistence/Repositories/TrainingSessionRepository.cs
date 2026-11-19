using FAMS.Application.Training.Common.Interfaces;
using FAMS.Domain.Training;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Persistence.Repositories;

public class TrainingSessionRepository : ITrainingSessionRepository
{
    private readonly ApplicationDbContext _context;

    public TrainingSessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<TrainingSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.TrainingSessions
            .Include(t => t.AttendanceRecords)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<TrainingSession> Sessions, int TotalCount)> SearchAsync(
        Guid? branchId,
        Guid? coachId,
        Guid? seasonId,
        DateTime? fromDate,
        DateTime? toDate,
        TrainingSessionStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.TrainingSessions.AsQueryable();

        if (branchId.HasValue) query = query.Where(t => t.BranchId == branchId.Value);
        if (coachId.HasValue) query = query.Where(t => t.CoachId == coachId.Value);
        if (seasonId.HasValue) query = query.Where(t => t.SeasonId == seasonId.Value);
        if (fromDate.HasValue) query = query.Where(t => t.SessionDate >= fromDate.Value.Date);
        if (toDate.HasValue) query = query.Where(t => t.SessionDate <= toDate.Value.Date);
        if (status.HasValue) query = query.Where(t => t.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var sessions = await query
            .Include(t => t.AttendanceRecords)
            .OrderBy(t => t.SessionDate)
            .ThenBy(t => t.StartTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (sessions, totalCount);
    }

    public async Task AddAsync(TrainingSession session, CancellationToken cancellationToken = default)
    {
        await _context.TrainingSessions.AddAsync(session, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TrainingSession> sessions, CancellationToken cancellationToken = default)
    {
        await _context.TrainingSessions.AddRangeAsync(sessions, cancellationToken);
    }

    public async Task AddAttendanceRecordAsync(AttendanceRecord record, CancellationToken cancellationToken = default)
    {
        // Records loaded with the session (via Include) are already tracked —
        // re-marking a player returns the tracked record, so only add when new.
        // Calling AddAsync on a tracked record flips it to Added and re-INSERTs
        // it, violating the PK.
        if (_context.Entry(record).State == EntityState.Detached)
        {
            await _context.AttendanceRecords.AddAsync(record, cancellationToken);
        }
    }
}
