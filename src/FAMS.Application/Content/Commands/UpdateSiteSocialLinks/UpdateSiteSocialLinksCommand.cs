using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.UpdateSiteSocialLinks;

public sealed record UpdateSiteSocialLinksCommand(
    string? FacebookUrl,
    string? InstagramUrl,
    string? YoutubeUrl,
    string? WhatsappUrl,
    string? LinkedinUrl) : IRequest<Result>;