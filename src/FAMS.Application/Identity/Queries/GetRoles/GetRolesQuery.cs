using FAMS.Application.Identity.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetRoles;

public record GetRolesQuery : IRequest<Result<IReadOnlyList<RoleDto>>>;
