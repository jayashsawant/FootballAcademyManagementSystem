using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.RemovePartnerLogo;

public sealed record RemovePartnerLogoCommand(Guid LogoId) : IRequest<Result>;