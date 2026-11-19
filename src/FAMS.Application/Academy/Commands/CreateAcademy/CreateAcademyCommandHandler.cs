using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateAcademy;

public class CreateAcademyCommandHandler : IRequestHandler<CreateAcademyCommand, Result<Guid>>
{
    private readonly IAcademyRepository _academyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAcademyCommandHandler(
        IAcademyRepository academyRepository,
        IUnitOfWork unitOfWork)
    {
        _academyRepository = academyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAcademyCommand request, CancellationToken cancellationToken)
    {
        var academyAlreadyExists = await _academyRepository.ExistsAsync(cancellationToken);

        if (academyAlreadyExists)
        {
            return Result<Guid>.Failure(
                Error.Conflict("Academy.AlreadyExists", "An academy has already been set up. Only one academy is supported at this time."));
        }

        var academy = Domain.Academy.Academy.Create(
            request.Name,
            request.Description,
            request.ContactEmail,
            request.ContactPhone,
            request.Address);

        await _academyRepository.AddAsync(academy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return academy.Id;
    }
}