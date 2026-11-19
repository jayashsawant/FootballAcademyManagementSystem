using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateBranch;

public class DeactivateBranchCommandHandler : IRequestHandler<DeactivateBranchCommand, Result>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateBranchCommandHandler(
        IBranchRepository branchRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        if (!branch.IsActive)
        {
            return Result.Failure(
                Error.Conflict("Branch.AlreadyInactive", "This branch is already deactivated."));
        }

        branch.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}