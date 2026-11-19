using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Auth.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<Result>;