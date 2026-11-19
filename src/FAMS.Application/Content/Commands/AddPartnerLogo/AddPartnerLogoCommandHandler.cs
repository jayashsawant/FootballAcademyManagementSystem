using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Application.Content.Common.Interfaces;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using MediatR;

namespace FAMS.Application.Content.Commands.AddPartnerLogo;

public class AddPartnerLogoCommandHandler : IRequestHandler<AddPartnerLogoCommand, Result<PartnerLogoDto>>
{
    private readonly IPartnerLogoRepository _partnerLogoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPartnerLogoCommandHandler(IPartnerLogoRepository partnerLogoRepository, IUnitOfWork unitOfWork)
    {
        _partnerLogoRepository = partnerLogoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PartnerLogoDto>> Handle(AddPartnerLogoCommand request, CancellationToken cancellationToken)
    {
        var logo = PartnerLogo.Create(request.LogoUrl, request.LinkUrl, request.SortOrder);

        await _partnerLogoRepository.AddAsync(logo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new PartnerLogoDto(logo.Id, logo.LogoUrl, logo.LinkUrl, logo.SortOrder);
    }
}