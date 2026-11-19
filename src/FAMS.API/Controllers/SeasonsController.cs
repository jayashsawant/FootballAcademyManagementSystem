using FAMS.API.Common;
using FAMS.Application.Academy.Commands.CreateSeason;
using FAMS.Application.Academy.Commands.SetCurrentSeason;
using FAMS.Application.Academy.Commands.UpdateSeason;
using FAMS.Application.Academy.Queries.GetCurrentSeason;
using FAMS.Application.Academy.Queries.GetSeasons;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/seasons")]
[Authorize(Roles = "Admin")]
public class SeasonsController : ControllerBase
{
    private readonly ISender _sender;

    public SeasonsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSeason(CreateSeasonCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetSeasons(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSeasonsQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentSeason(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentSeasonQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{seasonId:guid}")]
    public async Task<IActionResult> UpdateSeason(Guid seasonId, UpdateSeasonRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSeasonCommand(seasonId, request.Name, request.StartDate, request.EndDate);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{seasonId:guid}/set-current")]
    public async Task<IActionResult> SetCurrentSeason(Guid seasonId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SetCurrentSeasonCommand(seasonId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record UpdateSeasonRequest(string Name, DateTime StartDate, DateTime EndDate);