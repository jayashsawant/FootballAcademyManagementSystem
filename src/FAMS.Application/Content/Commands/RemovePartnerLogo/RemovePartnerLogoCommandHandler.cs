using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.RemovePartnerLogo;

public class RemovePartnerLogoCommandHandler : IRequestHandler<RemovePartnerLogoCommand, Result>
{
    private readonly IPartnerLogoRepository _partnerLogoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePartnerLogoCommandHandler(IPartnerLogoRepository partnerLogoRepository, IUnitOfWork unitOfWork)
    {
        _partnerLogoRepository = partnerLogoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemovePartnerLogoCommand request, CancellationToken cancellationToken)
    {
        var logo = await _partnerLogoRepository.GetByIdAsync(request.LogoId, cancellationToken);

        if (logo is null)
        {
            return Result.Failure(Error.NotFound("PartnerLogo.NotFound", "The specified partner logo could not be found."));
        }

        await _partnerLogoRepository.RemoveAsync(logo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}