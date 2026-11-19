using FAMS.Application.People.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.CreateParent;

public sealed record CreateParentCommand(
    string FirstName,
    string LastName,
    string Phone,
    string? Email,
    string? Address) : IRequest<Result<ParentDto>>;