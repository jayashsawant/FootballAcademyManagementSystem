using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetSeasons;

public sealed record GetSeasonsQuery : IRequest<Result<IReadOnlyList<SeasonDto>>>;