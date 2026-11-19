using FAMS.Application.Content.Common.Dtos;
using FAMS.Domain.Common;
using MediatR;

namespace FAMS.Application.Content.Commands.CreateNewsPost;

public sealed record CreateNewsPostCommand(string Title, string Slug, string Body, string? CoverImageUrl) : IRequest<Result<NewsPostDto>>;