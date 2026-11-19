using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetBatches;

public class GetBatchesQueryHandler : IRequestHandler<GetBatchesQuery, Result<IReadOnlyList<BatchDto>>>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;

    public GetBatchesQueryHandler(
        IBatchRepository batchRepository,
        IBranchRepository branchRepository,
        ISeasonRepository seasonRepository,
        IAgeGroupRepository ageGroupRepository)
    {
        _batchRepository = batchRepository;
        _branchRepository = branchRepository;
        _seasonRepository = seasonRepository;
        _ageGroupRepository = ageGroupRepository;
    }

    public async Task<Result<IReadOnlyList<BatchDto>>> Handle(GetBatchesQuery request, CancellationToken cancellationToken)
    {
        var batches = await _batchRepository.GetAllAsync(
            request.BranchId, request.SeasonId, request.AgeGroupId, request.IncludeInactive, cancellationToken);

        var dtos = new List<BatchDto>();

        foreach (var batch in batches)
        {
            var branch = await _branchRepository.GetByIdAsync(batch.BranchId, cancellationToken);
            var season = await _seasonRepository.GetByIdAsync(batch.SeasonId, cancellationToken);
            var ageGroup = await _ageGroupRepository.GetByIdAsync(batch.AgeGroupId, cancellationToken);

            dtos.Add(new BatchDto(
                batch.Id, batch.BranchId, branch?.Name ?? "Unknown",
                batch.SeasonId, season?.Name ?? "Unknown",
                batch.AgeGroupId, ageGroup?.Name ?? "Unknown",
                batch.Name, batch.IsActive));
        }

        return dtos;
    }
}