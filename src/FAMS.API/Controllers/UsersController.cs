using FAMS.API.Common;
using FAMS.Application.Identity.Commands.Users.ChangeUserRole;
using FAMS.Application.Identity.Commands.Users.DeactivateUser;
using FAMS.Application.Identity.Commands.Users.ReactivateUser;
using FAMS.Application.Identity.Commands.Users.ChangePassword;
using FAMS.Application.Identity.Commands.Users.UpdateProfile;
using FAMS.Application.Identity.Queries.GetCurrentUser;
using FAMS.Application.Identity.Queries.GetUserById;
using FAMS.Application.Identity.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentUserQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("me/change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery(userId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? roleId,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery(searchTerm, roleId, isActive, pageNumber, pageSize);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{userId:guid}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole(Guid userId, ChangeUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeUserRoleCommand(userId, request.NewRoleId);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{userId:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeactivateUserCommand(userId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{userId:guid}/reactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReactivateUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReactivateUserCommand(userId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record ChangeUserRoleRequest(Guid NewRoleId);