using FluentValidation;
using FAMS.Domain.People;

namespace FAMS.Application.People.Commands.CreatePlayer;

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty().WithMessage("Parent is required.");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch is required.");

        RuleFor(x => x.SeasonId)
            .NotEmpty().WithMessage("Season is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.UtcNow.Date).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Position)
            .Must(value => Enum.IsDefined(typeof(PlayerPosition), value))
            .WithMessage("Invalid player position.");

        RuleFor(x => x.JerseyNumber)
            .GreaterThan(0).WithMessage("Jersey number must be positive.")
            .When(x => x.JerseyNumber.HasValue);
    }
}