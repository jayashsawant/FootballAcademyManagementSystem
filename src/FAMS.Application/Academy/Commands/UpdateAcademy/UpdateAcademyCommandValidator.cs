using FluentValidation;

namespace FAMS.Application.Academy.Commands.UpdateAcademy;

public class UpdateAcademyCommandValidator : AbstractValidator<UpdateAcademyCommand>
{
    public UpdateAcademyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Academy name is required.")
            .MaximumLength(200).WithMessage("Academy name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.ContactEmail)
            .EmailAddress().WithMessage("Contact email must be a valid email address.")
            .MaximumLength(256).WithMessage("Contact email must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));

        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("Contact phone must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactPhone));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.")
            .When(x => x.Address is not null);
    }
}