using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateSeason;

public sealed record CreateSeasonCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate) : IRequest<Result<Guid>>;