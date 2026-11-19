using FAMS.Application.Common.Interfaces;
using FAMS.Application.People.Common.Dtos;
using FAMS.Application.People.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.People;
using MediatR;

namespace FAMS.Application.People.Commands.CreateParent;

public class CreateParentCommandHandler : IRequestHandler<CreateParentCommand, Result<ParentDto>>
{
    private readonly IParentRepository _parentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateParentCommandHandler(IParentRepository parentRepository, IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ParentDto>> Handle(CreateParentCommand request, CancellationToken cancellationToken)
    {
        var phoneExists = await _parentRepository.PhoneExistsAsync(request.Phone, cancellationToken: cancellationToken);

        if (phoneExists)
        {
            return Result<ParentDto>.Failure(
                Error.Conflict("Parent.PhoneInUse", "A parent with this phone number already exists."));
        }

        var parent = Parent.Create(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.Address);

        await _parentRepository.AddAsync(parent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ParentDto(
            parent.Id,
            parent.FirstName,
            parent.LastName,
            parent.Phone,
            parent.Email,
            parent.Address,
            parent.IsDeleted,
            parent.CreatedAt,
            new List<PlayerSummaryDto>());
    }
}
