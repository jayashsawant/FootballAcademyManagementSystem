using FAMS.Application.People.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetPlayerById;

public sealed record GetPlayerByIdQuery(Guid PlayerId) : IRequest<Result<PlayerDto>>;