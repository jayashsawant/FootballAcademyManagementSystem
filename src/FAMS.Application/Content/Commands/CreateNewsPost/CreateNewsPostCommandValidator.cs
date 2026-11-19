using FluentValidation;

namespace FAMS.Application.Content.Commands.CreateNewsPost;

public class CreateNewsPostCommandValidator : AbstractValidator<CreateNewsPostCommand>
{
    public CreateNewsPostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.").MaximumLength(200);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.").MaximumLength(200)
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase letters, numbers, and hyphens only.");
        RuleFor(x => x.Body).NotEmpty().WithMessage("Body is required.");
    }
}