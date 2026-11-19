using FluentValidation;

namespace FAMS.Application.Academy.Commands.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch is required.");
        RuleFor(x => x.SeasonId).NotEmpty().WithMessage("Season is required.");
        RuleFor(x => x.AgeGroupId).NotEmpty().WithMessage("Age group is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(100).WithMessage("Team name must not exceed 100 characters.");
    }
}