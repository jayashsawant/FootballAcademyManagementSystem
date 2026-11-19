using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.UpdatePlayer;

public sealed record UpdatePlayerCommand(
    Guid PlayerId,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    int Position,
    int? JerseyNumber) : IRequest<Result>;