using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.DeactivateUser;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Result>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserCommandHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == request.UserId)
        {
            return Result.Failure(
                Error.Conflict("User.CannotDeactivateSelf", "You cannot deactivate your own account."));
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", "User account could not be found."));
        }

        if (!user.IsActive)
        {
            return Result.Failure(
                Error.Conflict("User.AlreadyInactive", "This account is already deactivated."));
        }

        user.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}