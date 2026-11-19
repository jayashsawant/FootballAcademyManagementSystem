using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateAgeGroup;

public class ReactivateAgeGroupCommandHandler : IRequestHandler<ReactivateAgeGroupCommand, Result>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateAgeGroupCommandHandler(IAgeGroupRepository ageGroupRepository, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdIncludingDeletedAsync(request.AgeGroupId, cancellationToken);

        if (ageGroup is null)
        {
            return Result.Failure(Error.NotFound("AgeGroup.NotFound", "The specified age group could not be found."));
        }

        if (ageGroup.IsActive)
        {
            return Result.Failure(Error.Conflict("AgeGroup.AlreadyActive", "This age group is already active."));
        }

        ageGroup.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}