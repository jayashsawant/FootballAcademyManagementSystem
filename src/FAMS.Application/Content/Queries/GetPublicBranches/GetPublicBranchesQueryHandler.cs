using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicBranches;

public class GetPublicBranchesQueryHandler : IRequestHandler<GetPublicBranchesQuery, Result<IReadOnlyList<BranchDto>>>
{
    private readonly IBranchRepository _branchRepository;

    public GetPublicBranchesQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<IReadOnlyList<BranchDto>>> Handle(GetPublicBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepository.GetAllAsync(includeInactive: false, cancellationToken);

        var dtos = branches
            .Select(b => new BranchDto(
                b.Id, b.AcademyId, b.Name, b.Address, b.City, b.ContactPhone, b.ContactEmail,
                b.IsActive,
                b.Grounds
                    .Where(g => g.IsActive)
                    .Select(g => new GroundDto(g.Id, g.BranchId, g.Name, g.Description, g.IsActive))
                    .ToList(),
                b.CreatedAt))
            .ToList();

        return dtos;
    }
}