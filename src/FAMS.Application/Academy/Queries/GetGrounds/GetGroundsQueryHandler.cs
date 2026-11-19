using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetGrounds;

public class GetGroundsQueryHandler : IRequestHandler<GetGroundsQuery, Result<IReadOnlyList<GroundDto>>>
{
    private readonly IBranchRepository _branchRepository;

    public GetGroundsQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<IReadOnlyList<GroundDto>>> Handle(GetGroundsQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<IReadOnlyList<GroundDto>>.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        var groundDtos = branch.Grounds
            .Select(g => new GroundDto(g.Id, g.BranchId, g.Name, g.Description, g.IsActive))
            .ToList();

        return groundDtos;
    }
}
