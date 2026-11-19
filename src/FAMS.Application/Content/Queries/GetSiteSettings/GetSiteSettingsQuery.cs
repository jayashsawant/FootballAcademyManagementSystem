using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetSiteSettings;

public sealed record GetSiteSettingsQuery : IRequest<Result<SiteSettingsDto>>;