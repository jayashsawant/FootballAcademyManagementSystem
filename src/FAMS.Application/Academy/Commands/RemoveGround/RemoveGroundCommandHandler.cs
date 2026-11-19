using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.RemoveGround;

public class RemoveGroundCommandHandler : IRequestHandler<RemoveGroundCommand, Result>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveGroundCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveGroundCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        var groundExists = branch.Grounds.Any(g => g.Id == request.GroundId);

        if (!groundExists)
        {
            return Result.Failure(
                Error.NotFound("Ground.NotFound", "The specified ground could not be found on this branch."));
        }

        branch.RemoveGround(request.GroundId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}