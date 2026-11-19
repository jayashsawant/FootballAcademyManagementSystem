using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicAcademyInfo;

public class GetPublicAcademyInfoQueryHandler : IRequestHandler<GetPublicAcademyInfoQuery, Result<PublicAcademyInfoDto>>
{
    private readonly IAcademyRepository _academyRepository;

    public GetPublicAcademyInfoQueryHandler(IAcademyRepository academyRepository)
    {
        _academyRepository = academyRepository;
    }

    public async Task<Result<PublicAcademyInfoDto>> Handle(GetPublicAcademyInfoQuery request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result<PublicAcademyInfoDto>.Failure(Error.NotFound("Academy.NotFound", "Academy information is not configured."));
        }

        return new PublicAcademyInfoDto(academy.Name, academy.Description, academy.ContactEmail, academy.ContactPhone, academy.Address);
    }
}