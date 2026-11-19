using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicBatches;

public class GetPublicBatchesQueryHandler : IRequestHandler<GetPublicBatchesQuery, Result<IReadOnlyList<BatchDto>>>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IAcademyRepository _academyRepository;

    public GetPublicBatchesQueryHandler(
        IBatchRepository batchRepository,
        IBranchRepository branchRepository,
        IAgeGroupRepository ageGroupRepository,
        ISeasonRepository seasonRepository,
        IAcademyRepository academyRepository)
    {
        _batchRepository = batchRepository;
        _branchRepository = branchRepository;
        _ageGroupRepository = ageGroupRepository;
        _seasonRepository = seasonRepository;
        _academyRepository = academyRepository;
    }

    public async Task<Result<IReadOnlyList<BatchDto>>> Handle(GetPublicBatchesQuery request, CancellationToken cancellationToken)
    {
        var currentSeason = await _seasonRepository.GetCurrentAsync(cancellationToken);

        if (currentSeason is null)
        {
            return new List<BatchDto>();
        }

        var academy = await _academyRepository.GetAsync(cancellationToken);
        if (academy is null)
        {
            return new List<BatchDto>();
        }

        var batches = await _batchRepository.GetAllAsync(
            branchId: request.BranchId,
            seasonId: currentSeason.Id,
            ageGroupId: request.AgeGroupId,
            includeInactive: false,
            cancellationToken);

        if (batches.Count == 0)
        {
            return new List<BatchDto>();
        }

        var branches = await _branchRepository.GetAllAsync(includeInactive: false, cancellationToken);
        var branchNamesById = branches.ToDictionary(b => b.Id, b => b.Name);

        var ageGroups = await _ageGroupRepository.GetAllAsync(academy.Id, includeInactive: false, cancellationToken);
        var ageGroupNamesById = ageGroups.ToDictionary(a => a.Id, a => a.Name);

        var dtos = batches
            .Select(b => new BatchDto(
                b.Id,
                b.BranchId,
                branchNamesById.GetValueOrDefault(b.BranchId, "Unknown Branch"),
                b.SeasonId,
                currentSeason.Name,
                b.AgeGroupId,
                ageGroupNamesById.GetValueOrDefault(b.AgeGroupId, "Unknown Age Group"),
                b.Name,
                b.IsActive))
            .ToList();

        return dtos;
    }
}