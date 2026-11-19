using FAMS.API.Common;
using FAMS.Application.Academy.Commands.CreateAcademy;
using FAMS.Application.Academy.Commands.UpdateAcademy;
using FAMS.Application.Academy.Queries.GetAcademy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/academy")]
[Authorize(Roles = "Admin")]
public class AcademyController : ControllerBase
{
    private readonly ISender _sender;

    public AcademyController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAcademy(CreateAcademyCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAcademy(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAcademyQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAcademy(UpdateAcademyCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}