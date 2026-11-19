using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Identity;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("User.RoleNotFound", "The specified role does not exist."));
        }

        var phoneExists = await _userRepository.PhoneNumberExistsAsync(request.PhoneNumber, cancellationToken);

        if (phoneExists)
        {
            return Result<Guid>.Failure(
                Error.Conflict("User.PhoneNumberInUse", "This phone number is already registered."));
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _userRepository.EmailExistsAsync(request.Email, cancellationToken);

            if (emailExists)
            {
                return Result<Guid>.Failure(
                    Error.Conflict("User.EmailInUse", "This email address is already registered."));
            }
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.PhoneNumber,
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.RoleId,
            request.PersonReferenceId);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}