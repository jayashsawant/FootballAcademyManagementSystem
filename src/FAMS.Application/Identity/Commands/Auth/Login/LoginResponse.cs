namespace FAMS.Application.Identity.Commands.Auth.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt,
    string FirstName,
    string LastName,
    string RoleName);