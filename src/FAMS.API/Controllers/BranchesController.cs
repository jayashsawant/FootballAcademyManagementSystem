using FAMS.API.Common;
using FAMS.Application.Academy.Commands.AddGround;
using FAMS.Application.Academy.Commands.CreateBranch;
using FAMS.Application.Academy.Commands.DeactivateBranch;
using FAMS.Application.Academy.Commands.DeactivateGround;
using FAMS.Application.Academy.Commands.ReactivateBranch;
using FAMS.Application.Academy.Commands.ReactivateGround;
using FAMS.Application.Academy.Commands.RemoveGround;
using FAMS.Application.Academy.Commands.UpdateBranch;
using FAMS.Application.Academy.Commands.UpdateGround;
using FAMS.Application.Academy.Queries.GetBranchById;
using FAMS.Application.Academy.Queries.GetBranches;
using FAMS.Application.Academy.Queries.GetGroundById;
using FAMS.Application.Academy.Queries.GetGrounds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/branches")]
[Authorize(Roles = "Admin")]
public class BranchesController : ControllerBase
{
    private readonly ISender _sender;

    public BranchesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch(CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetBranches([FromQuery] bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetBranchesQuery(includeInactive), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{branchId:guid}")]
    public async Task<IActionResult> GetBranchById(Guid branchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBranchByIdQuery(branchId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{branchId:guid}")]
    public async Task<IActionResult> UpdateBranch(Guid branchId, UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBranchCommand(
            branchId, request.Name, request.Address, request.City, request.ContactPhone, request.ContactEmail);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{branchId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateBranch(Guid branchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateBranchCommand(branchId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{branchId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateBranch(Guid branchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateBranchCommand(branchId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{branchId:guid}/grounds")]
    public async Task<IActionResult> AddGround(Guid branchId, AddGroundRequest request, CancellationToken cancellationToken)
    {
        var command = new AddGroundCommand(branchId, request.Name, request.Description);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{branchId:guid}/grounds/{groundId:guid}")]
    public async Task<IActionResult> UpdateGround(Guid branchId, Guid groundId, UpdateGroundRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateGroundCommand(branchId, groundId, request.Name, request.Description);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{branchId:guid}/grounds/{groundId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateGround(Guid branchId, Guid groundId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateGroundCommand(branchId, groundId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{branchId:guid}/grounds/{groundId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateGround(Guid branchId, Guid groundId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateGroundCommand(branchId, groundId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{branchId:guid}/grounds")]
    public async Task<IActionResult> GetGrounds(Guid branchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGroundsQuery(branchId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{branchId:guid}/grounds/{groundId:guid}")]
    public async Task<IActionResult> GetGroundById(Guid branchId, Guid groundId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGroundByIdQuery(branchId, groundId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{branchId:guid}/grounds/{groundId:guid}")]
    public async Task<IActionResult> RemoveGround(Guid branchId, Guid groundId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RemoveGroundCommand(branchId, groundId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record AddGroundRequest(string Name, string? Description);
public sealed record UpdateBranchRequest(string Name, string? Address, string? City, string? ContactPhone, string? ContactEmail);
public sealed record UpdateGroundRequest(string Name, string? Description);