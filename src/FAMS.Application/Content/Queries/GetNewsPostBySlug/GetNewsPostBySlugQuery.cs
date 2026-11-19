using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Queries.GetNewsPostBySlug;

public sealed record GetNewsPostBySlugQuery(string Slug) : IRequest<Result<NewsPostDto>>;