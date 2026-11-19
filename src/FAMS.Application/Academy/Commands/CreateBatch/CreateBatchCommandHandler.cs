using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Academy;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateBatch;

public class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, Result<BatchDto>>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBatchCommandHandler(
        IBatchRepository batchRepository,
        IBranchRepository branchRepository,
        ISeasonRepository seasonRepository,
        IAgeGroupRepository ageGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _branchRepository = branchRepository;
        _seasonRepository = seasonRepository;
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BatchDto>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);

        if (branch is null)
        {
            return Result<BatchDto>.Failure(
                Error.NotFound("Batch.BranchNotFound", "The specified branch could not be found."));
        }

        var season = await _seasonRepository.GetByIdAsync(request.SeasonId, cancellationToken);

        if (season is null)
        {
            return Result<BatchDto>.Failure(
                Error.NotFound("Batch.SeasonNotFound", "The specified season could not be found."));
        }

        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.AgeGroupId, cancellationToken);

        if (ageGroup is null)
        {
            return Result<BatchDto>.Failure(
                Error.NotFound("Batch.AgeGroupNotFound", "The specified age group could not be found."));
        }

        if (ageGroup.AcademyId != branch.AcademyId)
        {
            return Result<BatchDto>.Failure(
                Error.Conflict("Batch.AgeGroupMismatch", "The specified age group does not belong to this branch's academy."));
        }

        var nameExists = await _batchRepository.NameExistsAsync(
            branch.AcademyId, request.BranchId, request.SeasonId, request.AgeGroupId, request.Name,
            cancellationToken: cancellationToken);

        if (nameExists)
        {
            return Result<BatchDto>.Failure(
                Error.Conflict("Batch.NameInUse", "A batch with this name already exists for this branch, season, and age group."));
        }

        var batch = Batch.Create(branch.AcademyId, request.BranchId, request.SeasonId, request.AgeGroupId, request.Name);

        await _batchRepository.AddAsync(batch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BatchDto(
            batch.Id, batch.BranchId, branch.Name, batch.SeasonId, season.Name,
            batch.AgeGroupId, ageGroup.Name, batch.Name, batch.IsActive);
    }
}