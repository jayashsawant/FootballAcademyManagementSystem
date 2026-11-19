using FAMS.Application.People.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Queries.GetParentById;

public sealed record GetParentByIdQuery(Guid ParentId) : IRequest<Result<ParentDto>>;