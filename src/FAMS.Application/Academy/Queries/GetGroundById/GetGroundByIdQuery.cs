using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetGroundById;

public sealed record GetGroundByIdQuery(Guid BranchId, Guid GroundId) : IRequest<Result<GroundDto>>;