using FluentValidation;

namespace FAMS.Application.Training.Commands.GenerateRecurringSessions;

public class GenerateRecurringSessionsCommandValidator : AbstractValidator<GenerateRecurringSessionsCommand>
{
    public GenerateRecurringSessionsCommandValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch is required.");
        RuleFor(x => x.GroundId).NotEmpty().WithMessage("Ground is required.");
        RuleFor(x => x.SeasonId).NotEmpty().WithMessage("Season is required.");
        RuleFor(x => x.CoachId).NotEmpty().WithMessage("Coach is required.");

        RuleFor(x => x.DaysOfWeek)
            .NotEmpty().WithMessage("At least one day of the week is required.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be on or after the start date.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays <= 180)
            .WithMessage("Date range cannot exceed 180 days.")
            .WithName("DateRange");
    }
}