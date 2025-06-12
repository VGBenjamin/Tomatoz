using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using System.Security.Claims;

namespace Tomatoz.Controllers.Api;

[ApiController]
[Route("api/v1/[controller]")]
public class TomatoVarietiesController : ControllerBase
{
    private readonly ITomatoVarietyService _varietyService;

    public TomatoVarietiesController(ITomatoVarietyService varietyService)
    {
        _varietyService = varietyService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<TomatoVarietyDto>>> Search([FromQuery] TomatoVarietySearchDto searchDto)
    {
        try
        {
            var result = await _varietyService.SearchAsync(searchDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TomatoVarietyDto>> GetById(int id)
    {
        var variety = await _varietyService.GetByIdAsync(id);
        if (variety == null)
            return NotFound();

        // Increment view count
        await _varietyService.IncrementViewCountAsync(id);

        return Ok(variety);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TomatoVarietyDto>> Create([FromBody] CreateTomatoVarietyDto createDto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var variety = await _varietyService.CreateAsync(createDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = variety.Id }, variety);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<TomatoVarietyVersionDto>> ProposeUpdate(int id, [FromBody] UpdateTomatoVarietyDto updateDto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            updateDto.Id = id;
            var version = await _varietyService.ProposeUpdateAsync(updateDto, userId);
            return Ok(version);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator,Moderator")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _varietyService.DeleteAsync(id, userId);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{id}/versions")]
    public async Task<ActionResult<List<TomatoVarietyVersionDto>>> GetVersions(int id)
    {
        var versions = await _varietyService.GetVersionsAsync(id);
        return Ok(versions);
    }

    [HttpPost("{id}/follow")]
    [Authorize]
    public async Task<ActionResult> Follow(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _varietyService.FollowVarietyAsync(id, userId);
        if (!success)
            return BadRequest("Already following or variety not found");

        return Ok();
    }

    [HttpDelete("{id}/follow")]
    [Authorize]
    public async Task<ActionResult> Unfollow(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _varietyService.UnfollowVarietyAsync(id, userId);
        if (!success)
            return BadRequest("Not following or variety not found");

        return Ok();
    }

    [HttpGet("followed")]
    [Authorize]
    public async Task<ActionResult<List<TomatoVarietyDto>>> GetFollowed()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var varieties = await _varietyService.GetFollowedVarietiesAsync(userId);
        return Ok(varieties);
    }
}
