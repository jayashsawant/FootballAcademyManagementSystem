using FAMS.Application.Common.Settings;
using FAMS.Domain.Common;
using MediatR;
using Microsoft.Extensions.Options;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;

namespace FAMS.Application.Identity.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private static readonly Error InvalidTokenError = Error.Unauthorized(
        "Auth.InvalidRefreshToken", "The refresh token is invalid or has expired.");

    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var incomingTokenHash = _refreshTokenGenerator.HashToken(request.RefreshToken);

        var user = await _userRepository.GetByRefreshTokenHashAsync(incomingTokenHash, cancellationToken);

        if (user is null)
        {
            return Result<RefreshTokenResponse>.Failure(InvalidTokenError);
        }

        var existingToken = user.FindActiveRefreshToken(incomingTokenHash);

        if (existingToken is null)
        {
            return Result<RefreshTokenResponse>.Failure(InvalidTokenError);
        }

        if (!user.IsActive)
        {
            return Result<RefreshTokenResponse>.Failure(
                Error.Unauthorized("Auth.AccountInactive", "This account has been deactivated."));
        }

        var role = await _roleRepository.GetByIdAsync(user.RoleId, cancellationToken);

        if (role is null)
        {
            return Result<RefreshTokenResponse>.Failure(
                Error.Failure("Auth.RoleNotFound", "The user's role could not be found."));
        }

        var newRawRefreshToken = _refreshTokenGenerator.GenerateToken();
        var newRefreshTokenHash = _refreshTokenGenerator.HashToken(newRawRefreshToken);
        var newRefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

        // Revoke the old token, linking it to its replacement.
        existingToken.Revoke(newRefreshTokenHash);

        // Create the new refresh token through the domain.
        var newRefreshToken = user.IssueRefreshToken(newRefreshTokenHash, newRefreshTokenExpiresAt);

        // Explicitly tell EF Core that this is a NEW entity (same fix as Login).
        await _userRepository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);

        var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user, role.Name);
        var newAccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(
            newAccessToken,
            newRawRefreshToken,
            newAccessTokenExpiresAt,
            newRefreshTokenExpiresAt);
    }
}