using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPartnerLogos;

public sealed record GetPartnerLogosQuery : IRequest<Result<IReadOnlyList<PartnerLogoDto>>>;