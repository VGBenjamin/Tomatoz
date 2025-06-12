using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using System.Security.Claims;

namespace Tomatoz.Controllers.Api;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Administrator,Moderator")]
public class ModerationController : ControllerBase
{
    private readonly IModerationService _moderationService;
    private readonly ITomatoVarietyService _varietyService;

    public ModerationController(
        IModerationService moderationService, 
        ITomatoVarietyService varietyService)
    {
        _moderationService = moderationService;
        _varietyService = varietyService;
    }

    [HttpGet("versions/pending")]
    public async Task<ActionResult<List<TomatoVarietyVersionDto>>> GetPendingVersions()
    {
        var versions = await _moderationService.GetPendingVersionsAsync();
        return Ok(versions);
    }

    [HttpGet("comments/pending")]
    public async Task<ActionResult<List<CommentDto>>> GetPendingComments()
    {
        var comments = await _moderationService.GetPendingCommentsAsync();
        return Ok(comments);
    }

    [HttpGet("photos/pending")]
    public async Task<ActionResult<List<TomatoPhotoDto>>> GetPendingPhotos()
    {
        var photos = await _moderationService.GetPendingPhotosAsync();
        return Ok(photos);
    }

    [HttpPut("versions/approve")]
    public async Task<ActionResult> ApproveVersions([FromBody] List<int> versionIds)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _moderationService.BulkApproveVersionsAsync(versionIds, userId);
        if (!success)
            return BadRequest("Failed to approve versions");

        return Ok();
    }

    [HttpPut("versions/reject")]
    public async Task<ActionResult> RejectVersions([FromBody] RejectVersionsDto rejectDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _moderationService.BulkRejectVersionsAsync(rejectDto.VersionIds, userId, rejectDto.Reason);
        if (!success)
            return BadRequest("Failed to reject versions");

        return Ok();
    }

    [HttpPut("versions/{id}/approve")]
    public async Task<ActionResult> ApproveVersion(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _varietyService.ApproveVersionAsync(id, userId);
        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpPut("versions/{id}/reject")]
    public async Task<ActionResult> RejectVersion(int id, [FromBody] string reason)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _varietyService.RejectVersionAsync(id, userId, reason);
        if (!success)
            return NotFound();

        return Ok();
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}/profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile(string id)
    {
        var profile = await _userService.GetProfileAsync(id);
        if (profile == null)
            return NotFound();

        return Ok(profile);
    }

    [HttpGet("me/profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var profile = await _userService.GetProfileAsync(userId);
        if (profile == null)
            return NotFound();

        return Ok(profile);
    }

    [HttpPut("me/profile")]
    [Authorize]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _userService.UpdateProfileAsync(userId, updateDto.DisplayName, updateDto.Bio);
        if (!success)
            return BadRequest("Failed to update profile");

        return Ok();
    }

    [HttpGet("me/activities")]
    [Authorize]
    public async Task<ActionResult<List<UserActivityDto>>> GetMyActivities([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var activities = await _userService.GetUserActivitiesAsync(userId, page, pageSize);
        return Ok(activities);
    }

    [HttpGet("me/points")]
    [Authorize]
    public async Task<ActionResult<int>> GetMyPoints()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var points = await _userService.GetUserPointsAsync(userId);
        return Ok(points);
    }

    [HttpGet("top-contributors")]
    public async Task<ActionResult<List<UserProfileDto>>> GetTopContributors([FromQuery] int count = 10)
    {
        var contributors = await _userService.GetTopContributorsAsync(count);
        return Ok(contributors);
    }
}

public class RejectVersionsDto
{
    public List<int> VersionIds { get; set; } = new();
    public string Reason { get; set; } = string.Empty;
}

public class UpdateProfileDto
{
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
}
