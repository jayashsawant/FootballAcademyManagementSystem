using FAMS.Application.Common.Settings;
using FAMS.Domain.Common;
using MediatR;
using Microsoft.Extensions.Options;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;

namespace FAMS.Application.Identity.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private static readonly Error InvalidCredentialsError = Error.Unauthorized(
        "Auth.InvalidCredentials",
        "Invalid phone number or password.");

    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByPhoneNumberAsync(
            request.PhoneNumber,
            cancellationToken);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(
                InvalidCredentialsError);
        }

        if (user.IsLockedOut)
        {
            return Result<LoginResponse>.Failure(
                Error.Unauthorized(
                    "Auth.AccountLocked",
                    "This account is temporarily locked due to repeated failed login attempts. Please try again later."));
        }

        if (!user.IsActive)
        {
            return Result<LoginResponse>.Failure(
                Error.Unauthorized(
                    "Auth.AccountInactive",
                    "This account has been deactivated."));
        }

        var passwordIsValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            user.RecordFailedLoginAttempt();

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result<LoginResponse>.Failure(
                InvalidCredentialsError);
        }

        user.ResetFailedLoginAttempts();

        var role = await _roleRepository.GetByIdAsync(
            user.RoleId,
            cancellationToken);

        if (role is null)
        {
            return Result<LoginResponse>.Failure(
                Error.Failure(
                    "Auth.RoleNotFound",
                    "The user's role could not be found."));
        }

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(
            user,
            role.Name);

        var accessTokenExpiresAt =
            DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpiryMinutes);

        var rawRefreshToken =
            _refreshTokenGenerator.GenerateToken();

        var refreshTokenHash =
            _refreshTokenGenerator.HashToken(
                rawRefreshToken);

        var refreshTokenExpiryDays = request.RememberMe
            ? _jwtSettings.RefreshTokenExpiryDaysRememberMe
            : _jwtSettings.RefreshTokenExpiryDays;

        var refreshTokenExpiresAt =
            DateTime.UtcNow.AddDays(
                refreshTokenExpiryDays);

        // Create the refresh token through the domain.
        var refreshToken = user.IssueRefreshToken(
            refreshTokenHash,
            refreshTokenExpiresAt);

        // Explicitly tell EF Core that this is a NEW entity.
        await _userRepository.AddRefreshTokenAsync(
            refreshToken,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new LoginResponse(
            accessToken,
            rawRefreshToken,
            accessTokenExpiresAt,
            refreshTokenExpiresAt,
            user.FirstName,
            user.LastName,
            role.Name);
    }
}