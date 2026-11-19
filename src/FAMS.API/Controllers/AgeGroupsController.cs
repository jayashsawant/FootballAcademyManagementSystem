using FAMS.API.Common;
using FAMS.Application.Academy.Commands.CreateAgeGroup;
using FAMS.Application.Academy.Commands.DeactivateAgeGroup;
using FAMS.Application.Academy.Commands.ReactivateAgeGroup;
using FAMS.Application.Academy.Commands.UpdateAgeGroup;
using FAMS.Application.Academy.Queries.GetAgeGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/age-groups")]
[Authorize(Roles = "Admin")]
public class AgeGroupsController : ControllerBase
{
    private readonly ISender _sender;

    public AgeGroupsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAgeGroup(CreateAgeGroupCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAgeGroups(
        [FromQuery] Guid academyId,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAgeGroupsQuery(academyId, includeInactive), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{ageGroupId:guid}")]
    public async Task<IActionResult> UpdateAgeGroup(Guid ageGroupId, UpdateAgeGroupRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateAgeGroupCommand(ageGroupId, request.Name, request.MinimumAge, request.MaximumAge, request.SortOrder);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{ageGroupId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateAgeGroup(Guid ageGroupId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateAgeGroupCommand(ageGroupId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{ageGroupId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateAgeGroup(Guid ageGroupId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateAgeGroupCommand(ageGroupId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record UpdateAgeGroupRequest(string Name, int? MinimumAge, int? MaximumAge, int SortOrder);