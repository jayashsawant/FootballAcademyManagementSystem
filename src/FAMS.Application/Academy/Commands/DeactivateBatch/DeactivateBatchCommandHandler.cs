using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateBatch;

public class DeactivateBatchCommandHandler : IRequestHandler<DeactivateBatchCommand, Result>
{
    private readonly IBatchRepository _batchRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateBatchCommandHandler(
        IBatchRepository batchRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.BatchId, cancellationToken);

        if (batch is null)
        {
            return Result.Failure(Error.NotFound("Batch.NotFound", "The specified batch could not be found."));
        }

        if (!batch.IsActive)
        {
            return Result.Failure(Error.Conflict("Batch.AlreadyInactive", "This batch is already deactivated."));
        }

        batch.Deactivate(_currentUserService.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}