using FAMS.Application.Billing.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Commands.RecordPayment;

public class RecordPaymentCommandHandler : IRequestHandler<RecordPaymentCommand, Result<Guid>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public RecordPaymentCommandHandler(
        IInvoiceRepository invoiceRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RecordPaymentCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);

        if (invoice is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("Payment.InvoiceNotFound", "The specified invoice could not be found."));
        }

        try
        {
            var payment = invoice.RecordPayment(
                request.Amount,
                request.Method,
                request.PaymentDate,
                request.ReferenceNumber,
                _currentUserService.UserId,
                request.Notes);

            await _invoiceRepository.AddPaymentAsync(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return payment.Id;
        }
        catch (InvalidOperationException ex)
        {
            return Result<Guid>.Failure(
                Error.Conflict("Payment.InvalidState", ex.Message));
        }
    }
}
