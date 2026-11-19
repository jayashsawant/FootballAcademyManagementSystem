using FluentValidation;

namespace FAMS.Application.Academy.Commands.AddGround;

public class AddGroundCommandValidator : AbstractValidator<AddGroundCommand>
{
    public AddGroundCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ground name is required.")
            .MaximumLength(200).WithMessage("Ground name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Description is not null);
    }
}