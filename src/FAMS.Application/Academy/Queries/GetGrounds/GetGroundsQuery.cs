using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetGrounds;

public sealed record GetGroundsQuery(Guid BranchId) : IRequest<Result<IReadOnlyList<GroundDto>>>;