using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Billing.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Billing;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Commands.GenerateMonthlyInvoices;

public class GenerateMonthlyInvoicesCommandHandler
    : IRequestHandler<GenerateMonthlyInvoicesCommand, Result<GenerateMonthlyInvoicesResult>>
{
    private const string InvoicePrefix = "INV";

    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAcademyRepository _academyRepository;
    private readonly IBusinessIdGenerator _businessIdGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateMonthlyInvoicesCommandHandler(
        IInvoiceRepository invoiceRepository,
        IPlayerRepository playerRepository,
        IAcademyRepository academyRepository,
        IBusinessIdGenerator businessIdGenerator,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _playerRepository = playerRepository;
        _academyRepository = academyRepository;
        _businessIdGenerator = businessIdGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GenerateMonthlyInvoicesResult>> Handle(
        GenerateMonthlyInvoicesCommand request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result<GenerateMonthlyInvoicesResult>.Failure(
                Error.NotFound("Invoice.AcademyNotFound", "No academy has been set up yet."));
        }

        var activePlayers = await _playerRepository.GetActiveAsync(cancellationToken);

        var invoicesCreated = 0;
        var playersSkipped = 0;

        foreach (var player in activePlayers)
        {
            var alreadyBilled = await _invoiceRepository.ExistsForPeriodAsync(
                player.Id, request.BillingYear, request.BillingMonth, cancellationToken);

            if (alreadyBilled)
            {
                playersSkipped++;
                continue;
            }

            var amount = player.MonthlyFeeOverride ?? academy.StandardMonthlyFee;

            if (amount <= 0)
            {
                playersSkipped++;
                continue;
            }

            var invoiceNumber = await _businessIdGenerator.GenerateAsync(InvoicePrefix, cancellationToken);

            var invoice = Invoice.Create(
                invoiceNumber,
                player.Id,
                request.SeasonId,
                request.BillingYear,
                request.BillingMonth,
                amount,
                request.DueDate);

            await _invoiceRepository.AddAsync(invoice, cancellationToken);
            invoicesCreated++;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new GenerateMonthlyInvoicesResult(invoicesCreated, playersSkipped);
    }
}