using FAMS.API.Common;
using FAMS.Application.People.Commands.CreateParent;
using FAMS.Application.People.Commands.DeactivateParent;
using FAMS.Application.People.Commands.ReactivateParent;
using FAMS.Application.People.Commands.UpdateParent;
using FAMS.Application.People.Queries.GetParentById;
using FAMS.Application.People.Queries.GetParents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/parents")]
[Authorize(Roles = "Admin,Receptionist")]
public class ParentsController : ControllerBase
{
    private readonly ISender _sender;

    public ParentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateParent(CreateParentCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{parentId:guid}")]
    public async Task<IActionResult> GetParentById(Guid parentId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetParentByIdQuery(parentId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetParents(
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetParentsQuery(searchTerm, isActive, pageNumber, pageSize), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{parentId:guid}")]
    public async Task<IActionResult> UpdateParent(Guid parentId, UpdateParentRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateParentCommand(parentId, request.FirstName, request.LastName, request.Phone, request.Email, request.Address);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{parentId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateParent(Guid parentId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateParentCommand(parentId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{parentId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateParent(Guid parentId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateParentCommand(parentId), cancellationToken);
        return result.ToActionResult();
    }
}
public sealed record UpdateParentRequest(string FirstName, string LastName, string Phone, string? Email, string? Address);
