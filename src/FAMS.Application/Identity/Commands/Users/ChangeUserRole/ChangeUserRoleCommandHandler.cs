using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Dtos;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.ChangeUserRole;

public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, Result<UserDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserRoleCommandHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == request.UserId)
        {
            return Result<UserDto>.Failure(
                Error.Conflict("User.CannotChangeOwnRole", "You cannot change your own role."));
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure(
                Error.NotFound("User.NotFound", "User account could not be found."));
        }

        if (!user.IsActive)
        {
            return Result<UserDto>.Failure(
                Error.Conflict("User.IsInactive", "This account has been deactivated. Reactivate it before changing its role."));
        }

        var newRole = await _roleRepository.GetByIdAsync(request.NewRoleId, cancellationToken);

        if (newRole is null)
        {
            return Result<UserDto>.Failure(
                Error.NotFound("User.RoleNotFound", "The specified role does not exist."));
        }

        user.ChangeRole(request.NewRoleId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserDto(
            user.Id,
            user.PhoneNumber,
            user.Email,
            user.FirstName,
            user.LastName,
            user.RoleId,
            newRole.Name,
            user.IsPhoneVerified,
            user.IsActive,
            user.CreatedAt);
    }
}
