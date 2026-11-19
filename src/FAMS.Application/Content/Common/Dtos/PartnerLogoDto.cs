namespace FAMS.Application.Content.Common.Dtos;

public sealed record PartnerLogoDto(Guid Id, string LogoUrl, string? LinkUrl, int SortOrder);