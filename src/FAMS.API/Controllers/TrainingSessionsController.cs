using FAMS.API.Common;
using FAMS.Application.Training.Commands.CreateTrainingSession;
using FAMS.Application.Training.Commands.GenerateRecurringSessions;
using FAMS.Application.Training.Commands.MarkAttendance;
using FAMS.Application.Training.Queries.GetTrainingSessions;
using FAMS.Domain.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/training-sessions")]
[Authorize(Roles = "Admin,Coach,BranchManager")]
public class TrainingSessionsController : ControllerBase
{
    private readonly ISender _sender;

    public TrainingSessionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrainingSession(CreateTrainingSessionCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("generate-recurring")]
    public async Task<IActionResult> GenerateRecurringSessions(GenerateRecurringSessionsCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetTrainingSessions(
        [FromQuery] Guid? branchId,
        [FromQuery] Guid? coachId,
        [FromQuery] Guid? seasonId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] TrainingSessionStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 100,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTrainingSessionsQuery(
            branchId, coachId, seasonId, fromDate, toDate, status, pageNumber, pageSize);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{trainingSessionId:guid}/attendance")]
    public async Task<IActionResult> MarkAttendance(Guid trainingSessionId, MarkAttendanceRequest request, CancellationToken cancellationToken)
    {
        var command = new MarkAttendanceCommand(trainingSessionId, request.PlayerId, request.Status);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record MarkAttendanceRequest(Guid PlayerId, FAMS.Domain.Training.AttendanceStatus Status);
