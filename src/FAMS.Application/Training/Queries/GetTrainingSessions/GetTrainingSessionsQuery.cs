using FAMS.Application.Common.Models;
using FAMS.Application.Training.Common.Dtos;
using FAMS.Domain.Common;
using FAMS.Domain.Training;
using MediatR;

namespace FAMS.Application.Training.Queries.GetTrainingSessions;

public record GetTrainingSessionsQuery(
    Guid? BranchId = null,
    Guid? CoachId = null,
    Guid? SeasonId = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    TrainingSessionStatus? Status = null,
    int PageNumber = 1,
    int PageSize = 100) : IRequest<Result<PagedResult<TrainingSessionDto>>>;
