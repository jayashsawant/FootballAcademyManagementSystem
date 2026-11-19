using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetGroundById;

public class GetGroundByIdQueryHandler : IRequestHandler<GetGroundByIdQuery, Result<GroundDto>>
{
    private readonly IBranchRepository _branchRepository;

    public GetGroundByIdQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<GroundDto>> Handle(GetGroundByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<GroundDto>.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        var ground = branch.Grounds.FirstOrDefault(g => g.Id == request.GroundId);

        if (ground is null)
        {
            return Result<GroundDto>.Failure(
                Error.NotFound("Ground.NotFound", "The specified ground could not be found on this branch."));
        }

        return new GroundDto(ground.Id, ground.BranchId, ground.Name, ground.Description, ground.IsActive);
    }
}
