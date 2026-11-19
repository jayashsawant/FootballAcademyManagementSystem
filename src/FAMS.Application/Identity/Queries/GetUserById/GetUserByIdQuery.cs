using FAMS.Application.Identity.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;