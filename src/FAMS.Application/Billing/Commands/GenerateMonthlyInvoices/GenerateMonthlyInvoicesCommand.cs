using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Commands.GenerateMonthlyInvoices;

public sealed record GenerateMonthlyInvoicesCommand(
    Guid SeasonId,
    int BillingYear,
    int BillingMonth,
    DateTime DueDate) : IRequest<Result<GenerateMonthlyInvoicesResult>>;

public sealed record GenerateMonthlyInvoicesResult(int InvoicesCreated, int PlayersSkipped);