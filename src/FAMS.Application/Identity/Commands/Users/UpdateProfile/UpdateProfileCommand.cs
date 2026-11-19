using FAMS.Application.Identity.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.UpdateProfile;

public sealed record UpdateProfileCommand(
    string FirstName,
    string LastName,
    string? Email) : IRequest<Result<UserDto>>;