using FAMS.Domain.Common;

namespace FAMS.Domain.Identity;

public class User : SoftDeletableEntity, IAggregateRoot
{
    private const int MaxFailedLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly List<RefreshToken> _refreshTokens = new();

    public string PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Guid RoleId { get; private set; }
    public Guid? PersonReferenceId { get; private set; }
    public bool IsPhoneVerified { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockoutEndAt { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    public bool IsLockedOut => LockoutEndAt is not null && LockoutEndAt > DateTime.UtcNow;

    private User(
        string phoneNumber,
        string? email,
        string passwordHash,
        string firstName,
        string lastName,
        Guid roleId,
        Guid? personReferenceId)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
        PersonReferenceId = personReferenceId;
        IsPhoneVerified = false;
        FailedLoginAttempts = 0;
        IsActive = true;
    }

    public static User Create(
        string phoneNumber,
        string? email,
        string passwordHash,
        string firstName,
        string lastName,
        Guid roleId,
        Guid? personReferenceId = null)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        return new User(
            phoneNumber.Trim(),
            email?.Trim(),
            passwordHash,
            firstName.Trim(),
            lastName.Trim(),
            roleId,
            personReferenceId);
    }

    public void UpdateProfile(string firstName, string lastName, string? email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email?.Trim();
    }

    public void ChangeRole(Guid newRoleId)
    {
        if (newRoleId == Guid.Empty)
            throw new ArgumentException("Role cannot be empty.", nameof(newRoleId));

        RoleId = newRoleId;
    }

    public void RecordFailedLoginAttempt()
    {
        if (IsLockedOut)
            return;

        FailedLoginAttempts++;

        if (FailedLoginAttempts >= MaxFailedLoginAttempts)
        {
            LockoutEndAt = DateTime.UtcNow.Add(LockoutDuration);
        }
    }

    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        LockoutEndAt = null;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        RevokeAllRefreshTokens();
    }

    public void VerifyPhone()
    {
        IsPhoneVerified = true;
    }

    public void Deactivate(Guid? deactivatedBy)
    {
        IsActive = false;
        RevokeAllRefreshTokens();
        Delete(deactivatedBy);
    }

    public void Reactivate()
    {
        IsActive = true;
        Restore();
    }

    public RefreshToken IssueRefreshToken(string tokenHash, DateTime expiresAt)
    {
        var refreshToken = RefreshToken.Create(Id, tokenHash, expiresAt);
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public RefreshToken? FindActiveRefreshToken(string tokenHash)
    {
        return _refreshTokens.FirstOrDefault(t => t.TokenHash == tokenHash && t.IsActive);
    }

    public void RevokeAllRefreshTokens()
    {
        foreach (var token in _refreshTokens.Where(t => t.IsActive))
        {
            token.Revoke();
        }
    }
}