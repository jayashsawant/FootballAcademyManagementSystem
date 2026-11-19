using FAMS.Application.Billing.Common.Dtos;
using FAMS.Application.Common.Models;
using FAMS.Domain.Billing;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Billing.Queries.GetInvoices;

public record GetInvoicesQuery(
    Guid? PlayerId = null,
    Guid? SeasonId = null,
    InvoiceStatus? Status = null,
    int? BillingYear = null,
    int? BillingMonth = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<PagedResult<InvoiceDto>>>;
