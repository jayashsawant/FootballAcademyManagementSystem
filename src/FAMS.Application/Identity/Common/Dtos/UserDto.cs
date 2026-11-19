namespace FAMS.Application.Identity.Common.Dtos;

public sealed record UserDto(
    Guid Id,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    Guid RoleId,
    string RoleName,
    bool IsPhoneVerified,
    bool IsActive,
    DateTime CreatedAt);
