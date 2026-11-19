using FluentValidation;

namespace FAMS.Application.Academy.Commands.UpdateSeason;

public class UpdateSeasonCommandValidator : AbstractValidator<UpdateSeasonCommand>
{
    public UpdateSeasonCommandValidator()
    {
        RuleFor(x => x.SeasonId)
            .NotEmpty().WithMessage("Season is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Season name is required.")
            .MaximumLength(100).WithMessage("Season name must not exceed 100 characters.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date.");
    }
}