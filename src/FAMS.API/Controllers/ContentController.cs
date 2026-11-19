using FAMS.API.Common;
using FAMS.Application.Content.Commands.AddGalleryImage;
using FAMS.Application.Content.Commands.AddPartnerLogo;
using FAMS.Application.Content.Commands.CreateNewsPost;
using FAMS.Application.Content.Commands.DeleteNewsPost;
using FAMS.Application.Content.Commands.PublishNewsPost;
using FAMS.Application.Content.Commands.RemoveGalleryImage;
using FAMS.Application.Content.Commands.RemovePartnerLogo;
using FAMS.Application.Content.Commands.UnpublishNewsPost;
using FAMS.Application.Content.Commands.UpdateNewsPost;
using FAMS.Application.Content.Commands.UpdateSiteBranding;
using FAMS.Application.Content.Commands.UpdateSiteSocialLinks;
using FAMS.Application.Content.Commands.UpdateTrialRequestStatus;
using FAMS.Application.Content.Queries.GetGalleryImages;
using FAMS.Application.Content.Queries.GetNewsPosts;
using FAMS.Application.Content.Queries.GetPartnerLogos;
using FAMS.Application.Content.Queries.GetSiteSettings;
using FAMS.Application.Content.Queries.GetTrialRequests;
using FAMS.Domain.Content;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/content")]
[Authorize(Roles = "Admin,ContentEditor")]
public class ContentController : ControllerBase
{
    private readonly ISender _sender;

    public ContentController(ISender sender)
    {
        _sender = sender;
    }

    // ---- Site settings ----

    [HttpGet("settings")]
    public async Task<IActionResult> GetSiteSettings(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSiteSettingsQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("settings/branding")]
    public async Task<IActionResult> UpdateBranding(UpdateSiteBrandingCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("settings/social")]
    public async Task<IActionResult> UpdateSocialLinks(UpdateSiteSocialLinksCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    // ---- Gallery ----

    [HttpGet("gallery")]
    public async Task<IActionResult> GetGalleryImages(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGalleryImagesQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("gallery")]
    public async Task<IActionResult> AddGalleryImage(AddGalleryImageCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("gallery/{imageId:guid}")]
    public async Task<IActionResult> RemoveGalleryImage(Guid imageId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RemoveGalleryImageCommand(imageId), cancellationToken);
        return result.ToActionResult();
    }

    // ---- News posts ----

    [HttpGet("news")]
    public async Task<IActionResult> GetNewsPosts(
        [FromQuery] NewsPostStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetNewsPostsQuery(status, pageNumber, pageSize), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("news")]
    public async Task<IActionResult> CreateNewsPost(CreateNewsPostCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("news/{postId:guid}")]
    public async Task<IActionResult> UpdateNewsPost(Guid postId, UpdateNewsPostRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateNewsPostCommand(postId, request.Title, request.Body, request.CoverImageUrl);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("news/{postId:guid}/publish")]
    public async Task<IActionResult> PublishNewsPost(Guid postId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new PublishNewsPostCommand(postId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("news/{postId:guid}/unpublish")]
    public async Task<IActionResult> UnpublishNewsPost(Guid postId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UnpublishNewsPostCommand(postId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("news/{postId:guid}")]
    public async Task<IActionResult> DeleteNewsPost(Guid postId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteNewsPostCommand(postId), cancellationToken);
        return result.ToActionResult();
    }

    // ---- Trial requests ----

    [HttpGet("trial-requests")]
    public async Task<IActionResult> GetTrialRequests(
        [FromQuery] TrialRequestStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetTrialRequestsQuery(status, pageNumber, pageSize), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("trial-requests/{requestId:guid}/status")]
    public async Task<IActionResult> UpdateTrialRequestStatus(Guid requestId, UpdateTrialRequestStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTrialRequestStatusCommand(requestId, request.Status, request.Notes);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    // ---- Partner logos ----

    [HttpGet("partners")]
    public async Task<IActionResult> GetPartnerLogos(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPartnerLogosQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("partners")]
    public async Task<IActionResult> AddPartnerLogo(AddPartnerLogoCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("partners/{logoId:guid}")]
    public async Task<IActionResult> RemovePartnerLogo(Guid logoId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RemovePartnerLogoCommand(logoId), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record UpdateNewsPostRequest(string Title, string Body, string? CoverImageUrl);
public sealed record UpdateTrialRequestStatusRequest(TrialRequestStatus Status, string? Notes);