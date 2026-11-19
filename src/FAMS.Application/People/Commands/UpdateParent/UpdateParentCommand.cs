using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.UpdateParent;

public sealed record UpdateParentCommand(
    Guid ParentId,
    string FirstName,
    string LastName,
    string Phone,
    string? Email,
    string? Address) : IRequest<Result>;