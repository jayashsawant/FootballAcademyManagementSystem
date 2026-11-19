using FAMS.API.Common;
using FAMS.Application.People.Commands.CreatePlayer;
using FAMS.Application.People.Commands.DeactivatePlayer;
using FAMS.Application.People.Commands.ReactivatePlayer;
using FAMS.Application.People.Commands.UpdatePlayer;
using FAMS.Application.People.Queries.GetPlayerById;
using FAMS.Application.People.Queries.GetPlayers;
using FAMS.Domain.People;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/players")]
[Authorize(Roles = "Admin,Receptionist")]
public class PlayersController : ControllerBase
{
    private readonly ISender _sender;

    public PlayersController(ISender sender)
    {
        _sender = sender;
    }

    // ---------------------------------------------------------
    // CREATE
    // ---------------------------------------------------------

    [HttpPost]
    public async Task<IActionResult> CreatePlayer(
        CreatePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    // ---------------------------------------------------------
    // GET BY ID
    // ---------------------------------------------------------

    [HttpGet("{playerId:guid}")]
    public async Task<IActionResult> GetPlayerById(
        Guid playerId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetPlayerByIdQuery(playerId),
            cancellationToken);

        return result.ToActionResult();
    }

    // ---------------------------------------------------------
    // GET PLAYERS
    // ---------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> GetPlayers(
    [FromQuery] string? searchTerm,
    [FromQuery] Guid? branchId,
    [FromQuery] Guid? seasonId,
    [FromQuery] Guid? ageGroupId,
    [FromQuery] Guid? batchId,
    [FromQuery] Guid? teamId,
    [FromQuery] Guid? parentId,
    [FromQuery] PlayerStatus? status,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var query = new GetPlayersQuery(
            searchTerm,
            branchId,
            seasonId,
            ageGroupId,
            batchId,
            teamId,
            parentId,
            status,
            pageNumber,
            pageSize);

        var result = await _sender.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }

    // ---------------------------------------------------------
    // UPDATE PLAYER PERSONAL DETAILS
    // ---------------------------------------------------------

    [HttpPut("{playerId:guid}")]
    public async Task<IActionResult> UpdatePlayer(
        Guid playerId,
        UpdatePlayerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePlayerCommand(
            playerId,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Position,
            request.JerseyNumber);

        var result = await _sender.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    // ---------------------------------------------------------
    // DEACTIVATE
    // ---------------------------------------------------------

    [HttpPost("{playerId:guid}/deactivate")]
    public async Task<IActionResult> DeactivatePlayer(
        Guid playerId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeactivatePlayerCommand(playerId),
            cancellationToken);

        return result.ToActionResult();
    }

    // ---------------------------------------------------------
    // REACTIVATE
    // ---------------------------------------------------------

    [HttpPost("{playerId:guid}/reactivate")]
    public async Task<IActionResult> ReactivatePlayer(
        Guid playerId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new ReactivatePlayerCommand(playerId),
            cancellationToken);

        return result.ToActionResult();
    }
}


// =============================================================
// REQUEST DTOs
// =============================================================

public sealed record UpdatePlayerRequest(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    int Position,
    int? JerseyNumber);