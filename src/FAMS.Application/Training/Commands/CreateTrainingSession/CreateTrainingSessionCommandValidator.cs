using FluentValidation;

namespace FAMS.Application.Training.Commands.CreateTrainingSession;

public class CreateTrainingSessionCommandValidator : AbstractValidator<CreateTrainingSessionCommand>
{
    public CreateTrainingSessionCommandValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch is required.");
        RuleFor(x => x.GroundId).NotEmpty().WithMessage("Ground is required.");
        RuleFor(x => x.SeasonId).NotEmpty().WithMessage("Season is required.");
        RuleFor(x => x.CoachId).NotEmpty().WithMessage("Coach is required.");

        RuleFor(x => x.SessionDate)
            .NotEmpty().WithMessage("Session date is required.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => x.Notes is not null);
    }
}