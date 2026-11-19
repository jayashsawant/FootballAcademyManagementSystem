using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetAcademy;

public sealed record GetAcademyQuery : IRequest<Result<AcademyDto>>;