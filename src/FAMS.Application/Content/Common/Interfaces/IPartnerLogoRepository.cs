using FAMS.Domain.Content;

namespace FAMS.Application.Content.Common.Interfaces;

public interface IPartnerLogoRepository
{
    Task<PartnerLogo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PartnerLogo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PartnerLogo logo, CancellationToken cancellationToken = default);
    Task RemoveAsync(PartnerLogo logo, CancellationToken cancellationToken = default);
}