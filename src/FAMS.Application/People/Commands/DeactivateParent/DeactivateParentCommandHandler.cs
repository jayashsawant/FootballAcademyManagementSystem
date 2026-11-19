using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.DeactivateParent;

public class DeactivateParentCommandHandler : IRequestHandler<DeactivateParentCommand, Result>
{
    private readonly IParentRepository _parentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateParentCommandHandler(
        IParentRepository parentRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateParentCommand request, CancellationToken cancellationToken)
    {
        var parent = await _parentRepository.GetByIdAsync(request.ParentId, cancellationToken);

        if (parent is null)
        {
            return Result.Failure(
                Error.NotFound("Parent.NotFound", "The specified parent could not be found."));
        }

        if (parent.IsDeleted)
        {
            return Result.Failure(
                Error.Conflict("Parent.AlreadyInactive", "This parent is already deactivated."));
        }

        parent.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}