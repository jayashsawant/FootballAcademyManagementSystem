using FluentValidation;

namespace FAMS.Application.Academy.Commands.CreateAgeGroup;

public class CreateAgeGroupCommandValidator : AbstractValidator<CreateAgeGroupCommand>
{
    public CreateAgeGroupCommandValidator()
    {
        RuleFor(x => x.AcademyId).NotEmpty().WithMessage("Academy is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Age group name is required.")
            .MaximumLength(50).WithMessage("Age group name must not exceed 50 characters.");

        RuleFor(x => x.MinimumAge)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum age cannot be negative.")
            .When(x => x.MinimumAge.HasValue);

        RuleFor(x => x.MaximumAge)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum age cannot be negative.")
            .When(x => x.MaximumAge.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinimumAge.HasValue || !x.MaximumAge.HasValue || x.MinimumAge <= x.MaximumAge)
            .WithMessage("Minimum age cannot be greater than maximum age.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Sort order cannot be negative.");
    }
}