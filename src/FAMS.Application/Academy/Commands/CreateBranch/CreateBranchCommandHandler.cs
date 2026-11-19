using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Academy;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateBranch;

public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, Result<Guid>>
{
    private readonly IAcademyRepository _academyRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBranchCommandHandler(
        IAcademyRepository academyRepository,
        IBranchRepository branchRepository,
        IUnitOfWork unitOfWork)
    {
        _academyRepository = academyRepository;
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("Academy.NotFound", "No academy has been set up yet. Create an academy before adding branches."));
        }

        var nameExists = await _branchRepository.NameExistsAsync(academy.Id, request.Name, null, cancellationToken);

        if (nameExists)
        {
            return Result<Guid>.Failure(
                Error.Conflict("Branch.NameInUse", "A branch with this name already exists."));
        }

        var branch = Branch.Create(
            academy.Id,
            request.Name,
            request.Address,
            request.City,
            request.ContactPhone,
            request.ContactEmail);

        await _branchRepository.AddAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return branch.Id;
    }
}