using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Coaches.Commands.UpdateCoach;

public sealed record UpdateCoachCommand(
    Guid CoachId,
    string FirstName,
    string LastName,
    string Phone,
    string? Email,
    string? Qualification,
    string? LicenseLevel,
    int? YearsExperience,
    int EmploymentType) : IRequest<Result>;