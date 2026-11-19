using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateAgeGroup;

public class UpdateAgeGroupCommandHandler : IRequestHandler<UpdateAgeGroupCommand, Result>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAgeGroupCommandHandler(IAgeGroupRepository ageGroupRepository, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.AgeGroupId, cancellationToken);

        if (ageGroup is null)
        {
            return Result.Failure(Error.NotFound("AgeGroup.NotFound", "The specified age group could not be found."));
        }

        var nameConflicts = await _ageGroupRepository.NameExistsAsync(
            ageGroup.AcademyId, request.Name, request.AgeGroupId, cancellationToken);

        if (nameConflicts)
        {
            return Result.Failure(Error.Conflict("AgeGroup.NameInUse", "An age group with this name already exists."));
        }

        ageGroup.UpdateDetails(request.Name, request.MinimumAge, request.MaximumAge, request.SortOrder);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}