using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.CreateUser;

public sealed record CreateUserCommand(
    string PhoneNumber,
    string? Email,
    string Password,
    string FirstName,
    string LastName,
    Guid RoleId,
    Guid? PersonReferenceId) : IRequest<Result<Guid>>;