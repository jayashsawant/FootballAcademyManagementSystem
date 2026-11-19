using FAMS.Application.Common.Models;
using FAMS.Application.Identity.Common.Dtos;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PagedResult<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public GetUsersQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<PagedResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.SearchAsync(
            request.SearchTerm,
            request.RoleId,
            request.IsActive,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var role = await _roleRepository.GetByIdAsync(user.RoleId, cancellationToken);
            var roleName = role?.Name ?? "Unknown";

            userDtos.Add(new UserDto(
                user.Id,
                user.PhoneNumber,
                user.Email,
                user.FirstName,
                user.LastName,
                user.RoleId,
                roleName,
                user.IsPhoneVerified,
                user.IsActive,
                user.CreatedAt));
        }

        var pagedResult = new PagedResult<UserDto>(
            userDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return pagedResult;
    }
}
