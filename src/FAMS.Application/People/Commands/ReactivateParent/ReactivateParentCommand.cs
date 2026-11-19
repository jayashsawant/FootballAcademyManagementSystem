using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.ReactivateParent;

public sealed record ReactivateParentCommand(Guid ParentId) : IRequest<Result>;