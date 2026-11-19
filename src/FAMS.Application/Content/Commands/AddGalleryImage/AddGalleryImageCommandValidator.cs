using FluentValidation;

namespace FAMS.Application.Content.Commands.AddGalleryImage;

public class AddGalleryImageCommandValidator : AbstractValidator<AddGalleryImageCommand>
{
    public AddGalleryImageCommandValidator()
    {
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Image URL is required.");
        RuleFor(x => x.Caption).MaximumLength(200).WithMessage("Caption must not exceed 200 characters.").When(x => x.Caption is not null);
    }
}