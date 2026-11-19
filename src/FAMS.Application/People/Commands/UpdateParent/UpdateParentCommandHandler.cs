using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.UpdateParent;

public class UpdateParentCommandHandler : IRequestHandler<UpdateParentCommand, Result>
{
    private readonly IParentRepository _parentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateParentCommandHandler(IParentRepository parentRepository, IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
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
                Error.Conflict("Parent.IsInactive", "This parent has been deactivated and cannot be edited. Reactivate them first."));
        }

        var phoneChanging = !string.Equals(request.Phone, parent.Phone, StringComparison.Ordinal);

        if (phoneChanging)
        {
            var phoneExists = await _parentRepository.PhoneExistsAsync(request.Phone, request.ParentId, cancellationToken);

            if (phoneExists)
            {
                return Result.Failure(
                    Error.Conflict("Parent.PhoneInUse", "A parent with this phone number already exists."));
            }
        }

        parent.UpdateDetails(request.FirstName, request.LastName, request.Phone, request.Email, request.Address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
