using FluentValidation;

namespace FAMS.Application.Identity.Commands.Users.ChangeUserRole;

public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required.");

        RuleFor(x => x.NewRoleId)
            .NotEmpty().WithMessage("Role is required.");
    }
}