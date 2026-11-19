using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.ReactivateUser;

public sealed record ReactivateUserCommand(Guid UserId) : IRequest<Result>;