using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateBranch;

public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, Result>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        var nameConflicts = await _branchRepository.NameExistsAsync(
            branch.AcademyId, request.Name, request.BranchId, cancellationToken);

        if (nameConflicts)
        {
            return Result.Failure(
                Error.Conflict("Branch.NameInUse", "A branch with this name already exists."));
        }

        branch.UpdateDetails(
            request.Name,
            request.Address,
            request.City,
            request.ContactPhone,
            request.ContactEmail);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}