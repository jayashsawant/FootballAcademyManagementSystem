using FAMS.Application.Identity.Common.Dtos;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<IReadOnlyList<RoleDto>>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<IReadOnlyList<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        var roleDtos = roles
            .Select(r => new RoleDto(r.Id, r.Name))
            .ToList();

        return roleDtos;
    }
}
