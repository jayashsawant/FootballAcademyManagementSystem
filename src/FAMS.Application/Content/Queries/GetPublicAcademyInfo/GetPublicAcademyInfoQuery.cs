using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicAcademyInfo;

public sealed record GetPublicAcademyInfoQuery : IRequest<Result<PublicAcademyInfoDto>>;