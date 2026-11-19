using FluentValidation;

namespace FAMS.Application.Academy.Commands.CreateBatch;

public class CreateBatchCommandValidator : AbstractValidator<CreateBatchCommand>
{
    public CreateBatchCommandValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch is required.");
        RuleFor(x => x.SeasonId).NotEmpty().WithMessage("Season is required.");
        RuleFor(x => x.AgeGroupId).NotEmpty().WithMessage("Age group is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Batch name is required.")
            .MaximumLength(100).WithMessage("Batch name must not exceed 100 characters.");
    }
}