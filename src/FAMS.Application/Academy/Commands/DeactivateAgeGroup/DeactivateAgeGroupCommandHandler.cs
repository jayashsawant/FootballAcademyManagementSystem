using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateAgeGroup;

public class DeactivateAgeGroupCommandHandler : IRequestHandler<DeactivateAgeGroupCommand, Result>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateAgeGroupCommandHandler(
        IAgeGroupRepository ageGroupRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.AgeGroupId, cancellationToken);

        if (ageGroup is null)
        {
            return Result.Failure(Error.NotFound("AgeGroup.NotFound", "The specified age group could not be found."));
        }

        if (!ageGroup.IsActive)
        {
            return Result.Failure(Error.Conflict("AgeGroup.AlreadyInactive", "This age group is already deactivated."));
        }

        ageGroup.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}