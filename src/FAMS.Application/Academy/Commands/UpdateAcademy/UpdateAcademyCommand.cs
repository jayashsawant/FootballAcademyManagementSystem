using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.UpdateAcademy;

public sealed record UpdateAcademyCommand(
    string Name,
    string? Description,
    string? ContactEmail,
    string? ContactPhone,
    string? Address) : IRequest<Result>;