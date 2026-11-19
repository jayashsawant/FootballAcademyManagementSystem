using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetPublicBranches;

public sealed record GetPublicBranchesQuery : IRequest<Result<IReadOnlyList<BranchDto>>>;