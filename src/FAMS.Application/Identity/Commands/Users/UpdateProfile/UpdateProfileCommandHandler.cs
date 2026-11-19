using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Dtos;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<UserDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
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

    public async Task<Result<UserDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
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

        if (!user.IsActive)
        {
            return Result<UserDto>.Failure(
                Error.Conflict("User.IsInactive", "This account has been deactivated and cannot be edited."));
        }

        var emailIsChanging = !string.IsNullOrWhiteSpace(request.Email)
            && !string.Equals(request.Email, user.Email, StringComparison.OrdinalIgnoreCase);

        if (emailIsChanging)
        {
            var emailExists = await _userRepository.EmailExistsAsync(request.Email!, cancellationToken);

            if (emailExists)
            {
                return Result<UserDto>.Failure(
                    Error.Conflict("User.EmailInUse", "This email address is already registered."));
            }
        }

        user.UpdateProfile(request.FirstName, request.LastName, request.Email);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
