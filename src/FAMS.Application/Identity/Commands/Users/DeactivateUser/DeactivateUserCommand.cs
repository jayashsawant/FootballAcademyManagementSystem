using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Result>;