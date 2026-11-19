using FluentValidation;

namespace FAMS.Application.Academy.Commands.UpdateBatch;

public class UpdateBatchCommandValidator : AbstractValidator<UpdateBatchCommand>
{
    public UpdateBatchCommandValidator()
    {
        RuleFor(x => x.BatchId).NotEmpty().WithMessage("Batch is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Batch name is required.")
            .MaximumLength(100).WithMessage("Batch name must not exceed 100 characters.");
    }
}