using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.ReactivateParent;

public class ReactivateParentCommandHandler : IRequestHandler<ReactivateParentCommand, Result>
{
    private readonly IParentRepository _parentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateParentCommandHandler(IParentRepository parentRepository, IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateParentCommand request, CancellationToken cancellationToken)
    {
        var parent = await _parentRepository.GetByIdIncludingDeletedAsync(request.ParentId, cancellationToken);

        if (parent is null)
        {
            return Result.Failure(
                Error.NotFound("Parent.NotFound", "The specified parent could not be found."));
        }

        if (!parent.IsDeleted)
        {
            return Result.Failure(
                Error.Conflict("Parent.AlreadyActive", "This parent is already active."));
        }

        parent.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}