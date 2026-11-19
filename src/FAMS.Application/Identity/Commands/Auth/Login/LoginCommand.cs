using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Auth.Login;

public sealed record LoginCommand(
    string PhoneNumber,
    string Password,
    bool RememberMe) : IRequest<Result<LoginResponse>>;