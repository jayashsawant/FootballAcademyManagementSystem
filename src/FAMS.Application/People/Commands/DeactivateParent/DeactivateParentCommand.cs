using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.DeactivateParent;

public sealed record DeactivateParentCommand(Guid ParentId) : IRequest<Result>;