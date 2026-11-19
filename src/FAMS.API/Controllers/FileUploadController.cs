using FAMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Controllers;

[ApiController]
[Route("api/content/files")]
[Authorize(Roles = "Admin,ContentEditor")]
public class FileUploadController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private static readonly string[] AllowedTypes = { "image/jpeg", "image/png", "image/webp", "image/gif" };
    private const long MaxSizeBytes = 5 * 1024 * 1024; // 5MB

    public FileUploadController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file provided.");

        if (file.Length > MaxSizeBytes)
            return BadRequest("File exceeds 5MB limit.");

        if (!AllowedTypes.Contains(file.ContentType))
            return BadRequest("Only JPEG, PNG, WEBP, or GIF images are allowed.");

        await using var stream = file.OpenReadStream();
        var url = await _fileStorageService.SaveAsync(stream, file.FileName, file.ContentType, cancellationToken);

        return Ok(new { url });
    }
}