using FAMS.Application.People.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.People.Commands.CreatePlayer;

public sealed record CreatePlayerCommand(
    Guid ParentId,
    Guid BranchId,
    Guid SeasonId,
    Guid AgeGroupId,
    Guid BatchId,
    Guid? TeamId,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    int Position,
    int? JerseyNumber,
    decimal? MonthlyFeeOverride = null)
    : IRequest<Result<PlayerDto>>;