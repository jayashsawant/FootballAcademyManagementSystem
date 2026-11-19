using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.ReactivateAgeGroup;

public sealed record ReactivateAgeGroupCommand(Guid AgeGroupId) : IRequest<Result>;