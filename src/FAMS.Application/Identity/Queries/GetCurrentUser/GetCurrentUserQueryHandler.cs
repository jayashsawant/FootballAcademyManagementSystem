using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Dtos;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public GetCurrentUserQueryHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            return Result<UserDto>.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "You must be logged in to perform this action."));
        }

        var user = await _userRepository.GetByIdAsync(_currentUserService.UserId.Value, cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure(
                Error.NotFound("User.NotFound", "User account could not be found."));
        }

        var role = await _roleRepository.GetByIdAsync(user.RoleId, cancellationToken);

        var roleName = role?.Name ?? "Unknown";

        return new UserDto(
            user.Id,
            user.PhoneNumber,
            user.Email,
            user.FirstName,
            user.LastName,
            user.RoleId,
            roleName,
            user.IsPhoneVerified,
            user.IsActive,
            user.CreatedAt);
    }
}
