using FAMS.Application.Coaches.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Queries.GetCoachById;

public sealed record GetCoachByIdQuery(Guid CoachId) : IRequest<Result<CoachDto>>;