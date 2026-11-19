using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.SubmitTrialRequest;

public sealed record SubmitTrialRequestCommand(string Name, DateTime DateOfBirth, string Email, string Mobile) : IRequest<Result<TrialRequestDto>>;