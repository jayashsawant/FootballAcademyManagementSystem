using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Commands.DeactivateAgeGroup;

public sealed record DeactivateAgeGroupCommand(Guid AgeGroupId) : IRequest<Result>;