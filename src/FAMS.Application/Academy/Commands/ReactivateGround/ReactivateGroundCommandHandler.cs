using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateGround;

public class ReactivateGroundCommandHandler : IRequestHandler<ReactivateGroundCommand, Result>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateGroundCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateGroundCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        var ground = branch.Grounds.FirstOrDefault(g => g.Id == request.GroundId);

        if (ground is null)
        {
            return Result.Failure(
                Error.NotFound("Ground.NotFound", "The specified ground could not be found on this branch."));
        }

        if (ground.IsActive)
        {
            return Result.Failure(
                Error.Conflict("Ground.AlreadyActive", "This ground is already active."));
        }

        ground.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}