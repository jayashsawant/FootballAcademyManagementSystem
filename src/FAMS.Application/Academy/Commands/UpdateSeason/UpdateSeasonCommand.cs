using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateSeason;

public sealed record UpdateSeasonCommand(
    Guid SeasonId,
    string Name,
    DateTime StartDate,
    DateTime EndDate) : IRequest<Result>;