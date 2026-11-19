using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.AddGround;

public class AddGroundCommandHandler : IRequestHandler<AddGroundCommand, Result<Guid>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddGroundCommandHandler(
        IBranchRepository branchRepository,
        IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddGroundCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("Branch.NotFound", "The specified branch could not be found."));
        }

        var ground = branch.AddGround(request.Name, request.Description);

        await _branchRepository.AddGroundAsync(ground, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ground.Id;
    }
}