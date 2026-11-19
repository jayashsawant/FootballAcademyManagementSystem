using FAMS.Application.Identity.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IRequest<Result<UserDto>>;