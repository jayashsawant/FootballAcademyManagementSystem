using FAMS.Application.Billing.Common.Dtos;
using FAMS.Application.Billing.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Billing;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoiceDto>>
{
    private const string InvoicePrefix = "INV";

    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IBusinessIdGenerator _businessIdGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IPlayerRepository playerRepository,
        IBusinessIdGenerator businessIdGenerator,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _playerRepository = playerRepository;
        _businessIdGenerator = businessIdGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InvoiceDto>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(request.PlayerId, cancellationToken);

        if (player is null)
        {
            return Result<InvoiceDto>.Failure(
                Error.NotFound("Invoice.PlayerNotFound", "The specified player could not be found."));
        }

        var alreadyBilled = await _invoiceRepository.ExistsForPeriodAsync(
            request.PlayerId, request.BillingYear, request.BillingMonth, cancellationToken);

        if (alreadyBilled)
        {
            return Result<InvoiceDto>.Failure(
                Error.Conflict("Invoice.AlreadyExists", "An invoice already exists for this player for the specified billing period."));
        }

        var invoiceNumber = await _businessIdGenerator.GenerateAsync(InvoicePrefix, cancellationToken);

        var invoice = Invoice.Create(
            invoiceNumber,
            request.PlayerId,
            request.SeasonId,
            request.BillingYear,
            request.BillingMonth,
            request.Amount,
            request.DueDate);

        await _invoiceRepository.AddAsync(invoice, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InvoiceDto(
            invoice.Id,
            invoice.InvoiceNumber,
            invoice.PlayerId,
            $"{player.FirstName} {player.LastName}",
            invoice.SeasonId,
            invoice.BillingYear,
            invoice.BillingMonth,
            invoice.Amount,
            invoice.AmountPaid,
            invoice.AmountDue,
            invoice.DueDate,
            invoice.Status.ToString(),
            new List<PaymentDto>());
    }
}