using FAMS.Application.Academy.Common.Interfaces;
using FAMS.Application.Common.Interfaces;
using FAMS.Domain.Academy;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateSeason;

public class CreateSeasonCommandHandler : IRequestHandler<CreateSeasonCommand, Result<Guid>>
{
    private readonly IAcademyRepository _academyRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSeasonCommandHandler(
        IAcademyRepository academyRepository,
        ISeasonRepository seasonRepository,
        IUnitOfWork unitOfWork)
    {
        _academyRepository = academyRepository;
        _seasonRepository = seasonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSeasonCommand request, CancellationToken cancellationToken)
    {
        var academy = await _academyRepository.GetAsync(cancellationToken);

        if (academy is null)
        {
            return Result<Guid>.Failure(
                Error.NotFound("Academy.NotFound", "No academy has been set up yet. Create an academy before adding seasons."));
        }

        var season = Season.Create(
            academy.Id,
            request.Name,
            request.StartDate,
            request.EndDate);

        await _seasonRepository.AddAsync(season, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return season.Id;
    }
}