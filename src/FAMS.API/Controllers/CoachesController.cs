using FAMS.API.Common;
using FAMS.Application.Coaches.Commands.CreateCoach;
using FAMS.Application.Coaches.Commands.DeactivateCoach;
using FAMS.Application.Coaches.Commands.ReactivateCoach;
using FAMS.Application.Coaches.Commands.TransferCoachToBranch;
using FAMS.Application.Coaches.Commands.UpdateCoach;
using FAMS.Application.Coaches.Queries.GetCoachById;
using FAMS.Application.Coaches.Queries.GetCoaches;
using FAMS.Domain.Coaches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/coaches")]
[Authorize(Roles = "Admin")]
public class CoachesController : ControllerBase
{
    private readonly ISender _sender;

    public CoachesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoach(CreateCoachCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{coachId:guid}")]
    public async Task<IActionResult> GetCoachById(Guid coachId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCoachByIdQuery(coachId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetCoaches(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? branchId,
        [FromQuery] CoachStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCoachesQuery(searchTerm, branchId, status, pageNumber, pageSize);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{coachId:guid}")]
    public async Task<IActionResult> UpdateCoach(Guid coachId, UpdateCoachRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCoachCommand(
            coachId, request.FirstName, request.LastName, request.Phone,
            request.Email, request.Qualification, request.LicenseLevel,
            request.YearsExperience, request.EmploymentType);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{coachId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCoach(Guid coachId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateCoachCommand(coachId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{coachId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateCoach(Guid coachId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateCoachCommand(coachId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{coachId:guid}/transfer")]
    public async Task<IActionResult> TransferCoachToBranch(Guid coachId, TransferCoachRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new TransferCoachToBranchCommand(coachId, request.NewBranchId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record UpdateCoachRequest(
    string FirstName, string LastName, string Phone,
    string? Email, string? Qualification, string? LicenseLevel,
    int? YearsExperience, int EmploymentType);

public sealed record TransferCoachRequest(Guid NewBranchId);