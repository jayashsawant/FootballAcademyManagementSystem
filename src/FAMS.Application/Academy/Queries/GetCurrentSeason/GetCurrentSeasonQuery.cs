using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetCurrentSeason;

public sealed record GetCurrentSeasonQuery : IRequest<Result<SeasonDto>>;