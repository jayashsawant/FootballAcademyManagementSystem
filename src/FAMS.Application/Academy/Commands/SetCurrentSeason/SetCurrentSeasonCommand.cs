using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.SetCurrentSeason;

public sealed record SetCurrentSeasonCommand(Guid SeasonId) : IRequest<Result>;