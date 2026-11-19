using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateSiteBranding;

public sealed record UpdateSiteBrandingCommand(
    string? LogoUrl,
    string? HeroImageUrl,
    string? HeroHeading,
    string? HeroSubtext,
    string? AboutText) : IRequest<Result>;