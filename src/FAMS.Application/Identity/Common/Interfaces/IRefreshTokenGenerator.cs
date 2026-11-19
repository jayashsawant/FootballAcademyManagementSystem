namespace FAMS.Application.Identity.Common.Interfaces;

public interface IRefreshTokenGenerator
{
    string GenerateToken();

    string HashToken(string token);
}