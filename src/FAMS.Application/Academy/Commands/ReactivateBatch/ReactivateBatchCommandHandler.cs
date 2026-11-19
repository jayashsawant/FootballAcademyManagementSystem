using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateBatch;

public class ReactivateBatchCommandHandler : IRequestHandler<ReactivateBatchCommand, Result>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateBatchCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReactivateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdIncludingDeletedAsync(request.BatchId, cancellationToken);

        if (batch is null)
        {
            return Result.Failure(Error.NotFound("Batch.NotFound", "The specified batch could not be found."));
        }

        if (batch.IsActive)
        {
            return Result.Failure(Error.Conflict("Batch.AlreadyActive", "This batch is already active."));
        }

        batch.Reactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}