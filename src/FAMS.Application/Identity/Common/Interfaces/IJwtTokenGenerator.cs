using FAMS.Domain.Identity;

namespace FAMS.Application.Identity.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user, string roleName);
}