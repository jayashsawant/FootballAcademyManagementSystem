using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Coaches.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.Training.Common.Dtos;
using FAMS.Application.Training.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Training;
using MediatR;

namespace FAMS.Application.Training.Commands.CreateTrainingSession;

public class CreateTrainingSessionCommandHandler : IRequestHandler<CreateTrainingSessionCommand, Result<TrainingSessionDto>>
{
    private readonly ITrainingSessionRepository _sessionRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrainingSessionCommandHandler(
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

    public async Task<Result<TrainingSessionDto>> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<TrainingSessionDto>.Failure(
                Error.NotFound("TrainingSession.BranchNotFound", "The specified branch could not be found."));
        }

        var ground = branch.Grounds.FirstOrDefault(g => g.Id == request.GroundId);

        if (ground is null)
        {
            return Result<TrainingSessionDto>.Failure(
                Error.NotFound("TrainingSession.GroundNotFound", "The specified ground could not be found on this branch."));
        }

        var coach = await _coachRepository.GetByIdAsync(request.CoachId, cancellationToken);

        if (coach is null)
        {
            return Result<TrainingSessionDto>.Failure(
                Error.NotFound("TrainingSession.CoachNotFound", "The specified coach could not be found."));
        }

        var session = TrainingSession.Create(
            request.BranchId,
            request.GroundId,
            request.SeasonId,
            request.CoachId,
            request.SessionDate,
            request.StartTime,
            request.EndTime,
            request.Notes);

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TrainingSessionDto(
            session.Id,
            session.BranchId,
            branch.Name,
            session.GroundId,
            ground.Name,
            session.SeasonId,
            session.CoachId,
            $"{coach.FirstName} {coach.LastName}",
            session.SessionDate,
            session.StartTime,
            session.EndTime,
            session.Status.ToString(),
            session.Notes,
            new List<AttendanceRecordDto>());
    }
}