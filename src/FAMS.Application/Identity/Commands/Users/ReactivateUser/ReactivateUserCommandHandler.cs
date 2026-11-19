using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.ReactivateUser;

public class ReactivateUserCommandHandler : IRequestHandler<ReactivateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdIncludingDeletedAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", "User account could not be found."));
        }

        if (user.IsActive)
        {
            return Result.Failure(
                Error.Conflict("User.AlreadyActive", "This account is already active."));
        }

        user.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}