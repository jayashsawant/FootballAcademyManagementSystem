using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetBranchById;

public sealed record GetBranchByIdQuery(Guid BranchId) : IRequest<Result<BranchDto>>;