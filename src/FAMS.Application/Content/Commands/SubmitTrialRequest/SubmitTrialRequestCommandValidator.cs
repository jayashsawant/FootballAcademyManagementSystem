using FluentValidation;

namespace FAMS.Application.Content.Commands.SubmitTrialRequest;

public class SubmitTrialRequestCommandValidator : AbstractValidator<SubmitTrialRequestCommand>
{
    public SubmitTrialRequestCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(100);
        RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.").LessThan(DateTime.UtcNow.Date);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Enter a valid email address.");
        RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile number is required.").Matches(@"^\+?[0-9]{10,15}$").WithMessage("Enter a valid mobile number.");
    }
}