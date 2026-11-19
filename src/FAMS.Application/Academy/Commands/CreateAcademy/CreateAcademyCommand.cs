using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.CreateAcademy;

public sealed record CreateAcademyCommand(
    string Name,
    string? Description,
    string? ContactEmail,
    string? ContactPhone,
    string? Address) : IRequest<Result<Guid>>;