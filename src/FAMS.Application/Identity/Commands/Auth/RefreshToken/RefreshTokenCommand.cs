using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;