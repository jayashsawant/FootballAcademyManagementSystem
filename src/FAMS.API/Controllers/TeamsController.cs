using FAMS.API.Common;
using FAMS.Application.Academy.Commands.CreateTeam;
using FAMS.Application.Academy.Commands.DeactivateTeam;
using FAMS.Application.Academy.Commands.ReactivateTeam;
using FAMS.Application.Academy.Commands.UpdateTeam;
using FAMS.Application.Academy.Queries.GetTeams;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/teams")]
[Authorize(Roles = "Admin")]
public class TeamsController : ControllerBase
{
    private readonly ISender _sender;

    public TeamsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam(CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetTeams(
        [FromQuery] Guid? branchId,
        [FromQuery] Guid? seasonId,
        [FromQuery] Guid? ageGroupId,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetTeamsQuery(branchId, seasonId, ageGroupId, includeInactive), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{teamId:guid}")]
    public async Task<IActionResult> UpdateTeam(Guid teamId, UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTeamCommand(teamId, request.Name);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{teamId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateTeam(Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateTeamCommand(teamId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{teamId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateTeam(Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateTeamCommand(teamId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record UpdateTeamRequest(string Name);