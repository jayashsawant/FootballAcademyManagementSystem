namespace FAMS.Application.People.Common.Dtos;

public sealed record PlayerDto(
    Guid Id,
    string PlayerNumber,

    Guid ParentId,
    string ParentName,

    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    int Age,

    string Position,
    int? JerseyNumber,

    Guid? EnrollmentId,

    Guid? SeasonId,
    string? SeasonName,

    Guid? BranchId,
    string? BranchName,

    Guid? AgeGroupId,
    string? AgeGroupName,

    Guid? BatchId,
    string? BatchName,

    Guid? TeamId,
    string? TeamName,

    string Status,
    bool IsDeleted,

    DateTime CreatedAt);