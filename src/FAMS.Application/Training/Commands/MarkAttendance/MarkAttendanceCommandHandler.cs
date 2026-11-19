using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Application.Training.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Training.Commands.MarkAttendance;

public class MarkAttendanceCommandHandler : IRequestHandler<MarkAttendanceCommand, Result<Guid>>
{
    private readonly ITrainingSessionRepository _sessionRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public MarkAttendanceCommandHandler(
        ITrainingSessionRepository sessionRepository,
        IPlayerRepository playerRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _playerRepository = playerRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.TrainingSessionId, cancellationToken);

        if (session is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("Attendance.SessionNotFound", "The specified training session could not be found."));
        }

        var player = await _playerRepository.GetByIdAsync(request.PlayerId, cancellationToken);

        if (player is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("Attendance.PlayerNotFound", "The specified player could not be found."));
        }

        var record = session.MarkAttendance(request.PlayerId, request.Status, _currentUserService.UserId);

        await _sessionRepository.AddAttendanceRecordAsync(record, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return record.Id;
    }
}