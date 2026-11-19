using FluentValidation;

namespace FAMS.Application.Billing.Commands.CreateInvoice;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.PlayerId).NotEmpty().WithMessage("Player is required.");
        RuleFor(x => x.SeasonId).NotEmpty().WithMessage("Season is required.");

        RuleFor(x => x.BillingYear)
            .InclusiveBetween(2020, 2100).WithMessage("Billing year is out of range.");

        RuleFor(x => x.BillingMonth)
            .InclusiveBetween(1, 12).WithMessage("Billing month must be between 1 and 12.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Invoice amount must be positive.");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required.");
    }
}