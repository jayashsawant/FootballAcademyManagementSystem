using FAMS.Application.Identity.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.ChangeUserRole;

public sealed record ChangeUserRoleCommand(Guid UserId, Guid NewRoleId) : IRequest<Result<UserDto>>;