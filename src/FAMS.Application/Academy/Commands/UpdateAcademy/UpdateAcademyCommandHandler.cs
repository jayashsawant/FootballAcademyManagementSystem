using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateAcademy;

public class UpdateAcademyCommandHandler : IRequestHandler<UpdateAcademyCommand, Result>
{
    private readonly IAcademyRepository _academyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAcademyCommandHandler(IAcademyRepository academyRepository, IUnitOfWork unitOfWork)
    {
        _academyRepository = academyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAcademyCommand request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result.Failure(
                Error.NotFound("Academy.NotFound", "No academy has been set up yet."));
        }

        academy.UpdateDetails(
            request.Name,
            request.Description,
            request.ContactEmail,
            request.ContactPhone,
            request.Address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}