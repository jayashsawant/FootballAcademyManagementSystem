using FluentValidation;

namespace FAMS.Application.Content.Commands.AddPartnerLogo;

public class AddPartnerLogoCommandValidator : AbstractValidator<AddPartnerLogoCommand>
{
    public AddPartnerLogoCommandValidator()
    {
        RuleFor(x => x.LogoUrl).NotEmpty().WithMessage("Logo URL is required.");
        RuleFor(x => x.LinkUrl).MaximumLength(500).When(x => x.LinkUrl is not null);
    }
}