using FAMS.Application.Coaches.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.CreateCoach;

public sealed record CreateCoachCommand(
    Guid BranchId,
    string FirstName,
    string LastName,
    string Phone,
    int EmploymentType,
    string? Email,
    string? Qualification,
    string? LicenseLevel,
    int? YearsExperience) : IRequest<Result<CoachDto>>;