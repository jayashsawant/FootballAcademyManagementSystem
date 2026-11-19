using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            return Result.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "You must be logged in to perform this action."));
        }

        var user = await _userRepository.GetByIdAsync(_currentUserService.UserId.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", "User account could not be found."));
        }

        var currentPasswordIsValid = _passwordHasher.Verify(request.CurrentPassword, user.PasswordHash);

        if (!currentPasswordIsValid)
        {
            return Result.Failure(
                Error.Unauthorized("Auth.InvalidCurrentPassword", "Your current password is incorrect."));
        }

        var newPasswordHash = _passwordHasher.Hash(request.NewPassword);

        user.ChangePassword(newPasswordHash);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
