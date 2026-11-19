using FAMS.API.Common;
using FAMS.Application.Billing.Commands.CreateInvoice;
using FAMS.Application.Billing.Commands.GenerateMonthlyInvoices;
using FAMS.Application.Billing.Commands.RecordPayment;
using FAMS.Application.Billing.Queries.GetInvoices;
using FAMS.Domain.Billing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/invoices")]
[Authorize(Roles = "Admin,FinanceOfficer")]
public class InvoicesController : ControllerBase
{
    private readonly ISender _sender;

    public InvoicesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("generate-monthly")]
    public async Task<IActionResult> GenerateMonthlyInvoices(GenerateMonthlyInvoicesCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices(
        [FromQuery] Guid? playerId,
        [FromQuery] Guid? seasonId,
        [FromQuery] InvoiceStatus? status,
        [FromQuery] int? billingYear,
        [FromQuery] int? billingMonth,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetInvoicesQuery(
            playerId, seasonId, status, billingYear, billingMonth, pageNumber, pageSize);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("{invoiceId:guid}/payments")]
    public async Task<IActionResult> RecordPayment(Guid invoiceId, RecordPaymentRequest request, CancellationToken cancellationToken)
    {
        var command = new RecordPaymentCommand(
            invoiceId, request.Amount, request.Method, request.PaymentDate, request.ReferenceNumber, request.Notes);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record RecordPaymentRequest(
    decimal Amount,
    FAMS.Domain.Billing.PaymentMethod Method,
    DateTime PaymentDate,
    string? ReferenceNumber,
    string? Notes);
