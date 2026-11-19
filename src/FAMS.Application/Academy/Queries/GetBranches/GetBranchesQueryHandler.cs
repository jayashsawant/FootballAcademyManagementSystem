using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetBranches;

public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, Result<IReadOnlyList<BranchDto>>>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchesQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<IReadOnlyList<BranchDto>>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepository.GetAllAsync(request.IncludeInactive, cancellationToken);

        var branchDtos = branches
            .Select(b => new BranchDto(
                b.Id,
                b.AcademyId,
                b.Name,
                b.Address,
                b.City,
                b.ContactPhone,
                b.ContactEmail,
                b.IsActive,
                b.Grounds.Select(g => new GroundDto(g.Id, g.BranchId, g.Name, g.Description, g.IsActive)).ToList(),
                b.CreatedAt))
            .ToList();

        return branchDtos;
    }
}
