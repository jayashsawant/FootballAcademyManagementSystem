namespace FAMS.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }

    string? RoleName { get; }

    bool IsAuthenticated { get; }
}