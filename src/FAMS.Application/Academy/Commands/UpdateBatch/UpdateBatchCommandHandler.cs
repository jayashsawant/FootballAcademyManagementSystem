using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateBatch;

public class UpdateBatchCommandHandler : IRequestHandler<UpdateBatchCommand, Result>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBatchCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.BatchId, cancellationToken);

        if (batch is null)
        {
            return Result.Failure(Error.NotFound("Batch.NotFound", "The specified batch could not be found."));
        }

        var nameConflicts = await _batchRepository.NameExistsAsync(
            batch.AcademyId, batch.BranchId, batch.SeasonId, batch.AgeGroupId, request.Name, request.BatchId, cancellationToken);

        if (nameConflicts)
        {
            return Result.Failure(Error.Conflict("Batch.NameInUse", "A batch with this name already exists for this branch, season, and age group."));
        }

        batch.UpdateDetails(batch.BranchId, batch.SeasonId, batch.AgeGroupId, request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}