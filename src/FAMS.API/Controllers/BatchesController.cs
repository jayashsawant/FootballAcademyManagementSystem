using FAMS.API.Common;
using FAMS.Application.Academy.Commands.CreateBatch;
using FAMS.Application.Academy.Commands.DeactivateBatch;
using FAMS.Application.Academy.Commands.ReactivateBatch;
using FAMS.Application.Academy.Commands.UpdateBatch;
using FAMS.Application.Academy.Queries.GetBatches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/batches")]
[Authorize(Roles = "Admin")]
public class BatchesController : ControllerBase
{
    private readonly ISender _sender;

    public BatchesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBatch(CreateBatchCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetBatches(
        [FromQuery] Guid? branchId,
        [FromQuery] Guid? seasonId,
        [FromQuery] Guid? ageGroupId,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetBatchesQuery(branchId, seasonId, ageGroupId, includeInactive), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{batchId:guid}")]
    public async Task<IActionResult> UpdateBatch(Guid batchId, UpdateBatchRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBatchCommand(batchId, request.Name);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{batchId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateBatch(Guid batchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateBatchCommand(batchId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{batchId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateBatch(Guid batchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateBatchCommand(batchId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record UpdateBatchRequest(string Name);