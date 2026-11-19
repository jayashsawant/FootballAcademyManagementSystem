using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Academy;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateAgeGroup;

public class CreateAgeGroupCommandHandler : IRequestHandler<CreateAgeGroupCommand, Result<AgeGroupDto>>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAgeGroupCommandHandler(IAgeGroupRepository ageGroupRepository, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AgeGroupDto>> Handle(CreateAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await _ageGroupRepository.NameExistsAsync(request.AcademyId, request.Name, cancellationToken: cancellationToken);

        if (nameExists)
        {
            return Result<AgeGroupDto>.Failure(
                Error.Conflict("AgeGroup.NameInUse", "An age group with this name already exists."));
        }

        var ageGroup = AgeGroup.Create(
            request.AcademyId, request.Name, request.MinimumAge, request.MaximumAge, request.SortOrder);

        await _ageGroupRepository.AddAsync(ageGroup, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AgeGroupDto(
            ageGroup.Id, ageGroup.AcademyId, ageGroup.Name,
            ageGroup.MinimumAge, ageGroup.MaximumAge, ageGroup.SortOrder, ageGroup.IsActive);
    }
}