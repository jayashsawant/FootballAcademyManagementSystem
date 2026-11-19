using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetBranchById;

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, Result<BranchDto>>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchByIdQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<BranchDto>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<BranchDto>.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        return new BranchDto(
            branch.Id,
            branch.AcademyId,
            branch.Name,
            branch.Address,
            branch.City,
            branch.ContactPhone,
            branch.ContactEmail,
            branch.IsActive,
            branch.Grounds.Select(g => new GroundDto(g.Id, g.BranchId, g.Name, g.Description, g.IsActive)).ToList(),
            branch.CreatedAt);
    }
}
