using FAMS.Application.Academy.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Academy.Queries.GetBranches;

public sealed record GetBranchesQuery(bool IncludeInactive = false) : IRequest<Result<IReadOnlyList<BranchDto>>>;