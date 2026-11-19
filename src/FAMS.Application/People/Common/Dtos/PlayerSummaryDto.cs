namespace FAMS.Application.People.Common.Dtos;

public sealed record PlayerSummaryDto(
    Guid Id,
    string PlayerNumber,
    string FirstName,
    string LastName,
    string Status);
