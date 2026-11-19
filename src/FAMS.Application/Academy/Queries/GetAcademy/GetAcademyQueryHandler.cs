using FAMS.Application.Academy.Common.Dtos;
using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetAcademy;

public class GetAcademyQueryHandler : IRequestHandler<GetAcademyQuery, Result<AcademyDto>>
{
    private readonly IAcademyRepository _academyRepository;

    public GetAcademyQueryHandler(IAcademyRepository academyRepository)
    {
        _academyRepository = academyRepository;
    }

    public async Task<Result<AcademyDto>> Handle(GetAcademyQuery request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result<AcademyDto>.Failure(
                Error.NotFound("Academy.NotFound", "No academy has been set up yet."));
        }

        return new AcademyDto(
            academy.Id,
            academy.Name,
            academy.Description,
            academy.ContactEmail,
            academy.ContactPhone,
            academy.Address);
    }
}