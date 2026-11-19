using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.AddPartnerLogo;

public sealed record AddPartnerLogoCommand(string LogoUrl, string? LinkUrl, int SortOrder) : IRequest<Result<PartnerLogoDto>>;