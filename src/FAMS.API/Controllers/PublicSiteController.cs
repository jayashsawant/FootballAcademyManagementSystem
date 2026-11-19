using FAMS.API.Common;
using FAMS.Application.Content.Commands.SubmitTrialRequest;
using FAMS.Application.Content.Queries.GetGalleryImages;
using FAMS.Application.Content.Queries.GetNewsPostBySlug;
using FAMS.Application.Content.Queries.GetNewsPosts;
using FAMS.Application.Content.Queries.GetPartnerLogos;
using FAMS.Application.Content.Queries.GetPublicAcademyInfo;
using FAMS.Application.Content.Queries.GetPublicAgeGroups;
using FAMS.Application.Content.Queries.GetPublicBatches;
using FAMS.Application.Content.Queries.GetPublicBranches;
using FAMS.Application.Content.Queries.GetSiteSettings;
using FAMS.Domain.Content;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/public/site")]
public class PublicSiteController : ControllerBase
{
    private readonly ISender _sender;

    public PublicSiteController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("settings")]
    public async Task<IActionResult> GetSiteSettings(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSiteSettingsQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("gallery")]
    public async Task<IActionResult> GetGalleryImages(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGalleryImagesQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("news")]
    public async Task<IActionResult> GetNewsPosts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetNewsPostsQuery(NewsPostStatus.Published, pageNumber, pageSize), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("news/{slug}")]
    public async Task<IActionResult> GetNewsPostBySlug(string slug, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetNewsPostBySlugQuery(slug), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("trial-requests")]
    public async Task<IActionResult> SubmitTrialRequest(SubmitTrialRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("partners")]
    public async Task<IActionResult> GetPartnerLogos(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPartnerLogosQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("academy-info")]
    public async Task<IActionResult> GetAcademyInfo(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPublicAcademyInfoQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("branches")]
    public async Task<IActionResult> GetBranches(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPublicBranchesQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("age-groups")]
    public async Task<IActionResult> GetAgeGroups(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPublicAgeGroupsQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("batches")]
    public async Task<IActionResult> GetBatches([FromQuery] Guid? branchId, [FromQuery] Guid? ageGroupId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPublicBatchesQuery(branchId, ageGroupId), cancellationToken);
        return result.ToActionResult();
    }
}