using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Enums;
using System.Security.Claims;

namespace Tomatoz.Controllers.Api;

[ApiController]
[Route("api/v1/[controller]")]
public class PhotosController : ControllerBase
{
    private readonly ITomatoPhotoService _photoService;
    private readonly IWebHostEnvironment _environment;

    public PhotosController(ITomatoPhotoService photoService, IWebHostEnvironment environment)
    {
        _photoService = photoService;
        _environment = environment;
    }

    [HttpGet("variety/{varietyId}")]
    public async Task<ActionResult<List<TomatoPhotoDto>>> GetByVariety(int varietyId)
    {
        var photos = await _photoService.GetByVarietyIdAsync(varietyId);
        return Ok(photos);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TomatoPhotoDto>> Upload([FromForm] UploadPhotoDto uploadDto, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var photo = await _photoService.UploadAsync(uploadDto, file.OpenReadStream(), file.FileName, userId);
            return CreatedAtAction(nameof(GetByVariety), new { varietyId = uploadDto.TomatoVarietyId }, photo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _photoService.DeleteAsync(id, userId);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Administrator,Moderator")]
    public async Task<ActionResult> Approve(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _photoService.ApproveAsync(id, userId);
        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpPut("{id}/order")]
    [Authorize]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] int newOrder)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _photoService.UpdateOrderAsync(id, newOrder, userId);
        if (!success)
            return NotFound();

        return Ok();
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class VotesController : ControllerBase
{
    private readonly IVoteService _voteService;

    public VotesController(IVoteService voteService)
    {
        _voteService = voteService;
    }

    [HttpGet("variety/{varietyId}/user")]
    [Authorize]
    public async Task<ActionResult<VoteDto>> GetUserVote(int varietyId, [FromQuery] VoteType voteType)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var vote = await _voteService.GetUserVoteAsync(varietyId, userId, voteType);
        if (vote == null)
            return NotFound();

        return Ok(vote);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<VoteDto>> CreateOrUpdate([FromBody] CreateVoteDto voteDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var vote = await _voteService.CreateOrUpdateVoteAsync(voteDto, userId);
            return Ok(vote);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("variety/{varietyId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int varietyId, [FromQuery] VoteType voteType)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _voteService.DeleteVoteAsync(varietyId, userId, voteType);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("variety/{varietyId}/averages")]
    public async Task<ActionResult<Dictionary<VoteType, decimal>>> GetAverageRatings(int varietyId)
    {
        var averages = await _voteService.GetAverageRatingsAsync(varietyId);
        return Ok(averages);
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("variety/{varietyId}")]
    public async Task<ActionResult<List<CommentDto>>> GetByVariety(int varietyId)
    {
        var comments = await _commentService.GetByVarietyIdAsync(varietyId);
        return Ok(comments);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto commentDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var comment = await _commentService.CreateAsync(commentDto, userId);
            return CreatedAtAction(nameof(GetByVariety), new { varietyId = commentDto.TomatoVarietyId }, comment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, [FromBody] string content)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _commentService.UpdateAsync(id, content, userId);
        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _commentService.DeleteAsync(id, userId);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Administrator,Moderator")]
    public async Task<ActionResult> Approve(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _commentService.ApproveAsync(id, userId);
        if (!success)
            return NotFound();

        return Ok();
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TagDto>>> GetAll()
    {
        var tags = await _tagService.GetAllAsync();
        return Ok(tags);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<TagDto>>> Search([FromQuery] string query)
    {
        var tags = await _tagService.SearchAsync(query);
        return Ok(tags);
    }

    [HttpGet("popular")]
    public async Task<ActionResult<List<TagDto>>> GetPopular([FromQuery] int count = 20)
    {
        var tags = await _tagService.GetPopularTagsAsync(count);
        return Ok(tags);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TagDto>> Create([FromBody] CreateTagDto createDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var tag = await _tagService.CreateAsync(createDto.Name, createDto.Description, userId);
            return CreatedAtAction(nameof(GetAll), null, tag);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class CreateTagDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
