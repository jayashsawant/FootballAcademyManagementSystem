using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Identity.Commands.Users.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword) : IRequest<Result>;