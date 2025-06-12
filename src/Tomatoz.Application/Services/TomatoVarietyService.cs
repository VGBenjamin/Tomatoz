using AutoMapper;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Enums;
using Tomatoz.Shared.Models;

namespace Tomatoz.Application.Services;

public class TomatoVarietyService : ITomatoVarietyService
{
    private readonly ITomatoVarietyRepository _varietyRepository;
    private readonly ITomatoVarietyVersionRepository _versionRepository;
    private readonly IUserFollowsVarietyRepository _followRepository;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public TomatoVarietyService(
        ITomatoVarietyRepository varietyRepository,
        ITomatoVarietyVersionRepository versionRepository,
        IUserFollowsVarietyRepository followRepository,
        IUserService userService,
        INotificationService notificationService,
        IMapper mapper)
    {
        _varietyRepository = varietyRepository;
        _versionRepository = versionRepository;
        _followRepository = followRepository;
        _userService = userService;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task<PaginatedResultDto<TomatoVarietyDto>> SearchAsync(TomatoVarietySearchDto searchDto)
    {
        var varieties = await _varietyRepository.SearchAsync(
            searchDto.SearchTerm,
            searchDto.GrowthType,
            searchDto.Color,
            searchDto.Shape,
            searchDto.Size,
            searchDto.DiseaseResistance,
            searchDto.Tags,
            searchDto.Page,
            searchDto.PageSize,
            searchDto.SortBy,
            searchDto.SortDescending);

        var totalCount = await _varietyRepository.GetSearchCountAsync(
            searchDto.SearchTerm,
            searchDto.GrowthType,
            searchDto.Color,
            searchDto.Shape,
            searchDto.Size,
            searchDto.DiseaseResistance,
            searchDto.Tags);

        var varietyDtos = _mapper.Map<List<TomatoVarietyDto>>(varieties);

        return new PaginatedResultDto<TomatoVarietyDto>
        {
            Items = varietyDtos,
            TotalCount = totalCount,
            Page = searchDto.Page,
            PageSize = searchDto.PageSize
        };
    }

    public async Task<TomatoVarietyDto?> GetByIdAsync(int id)
    {
        var variety = await _varietyRepository.GetWithDetailsAsync(id);
        return variety != null ? _mapper.Map<TomatoVarietyDto>(variety) : null;
    }

    public async Task<TomatoVarietyDto> CreateAsync(CreateTomatoVarietyDto createDto, string userId)
    {
        var variety = _mapper.Map<TomatoVariety>(createDto);
        variety.CreatedAt = DateTime.UtcNow;
        
        var createdVariety = await _varietyRepository.AddAsync(variety);

        // Create the initial version
        var version = new TomatoVarietyVersion
        {
            TomatoVarietyId = createdVariety.Id,
            UserId = userId,
            VersionNumber = 1,
            Status = ContributionStatus.Published,
            CreatedAt = DateTime.UtcNow,
            VersionData = System.Text.Json.JsonSerializer.Serialize(createDto),
            ChangeDescription = "Initial version"
        };

        await _versionRepository.AddAsync(version);

        // Award points for creation
        await _userService.AddUserActivityAsync(userId, ActivityType.CreateVariety, 10, createdVariety.Id);

        return _mapper.Map<TomatoVarietyDto>(createdVariety);
    }

    public async Task<TomatoVarietyVersionDto> ProposeUpdateAsync(UpdateTomatoVarietyDto updateDto, string userId)
    {
        var variety = await _varietyRepository.GetByIdAsync(updateDto.Id);
        if (variety == null)
            throw new ArgumentException("Variety not found");

        var latestVersion = await _versionRepository.GetLatestApprovedVersionAsync(updateDto.Id);
        var newVersionNumber = latestVersion?.VersionNumber + 1 ?? 1;

        var version = new TomatoVarietyVersion
        {
            TomatoVarietyId = updateDto.Id,
            UserId = userId,
            VersionNumber = newVersionNumber,
            Status = ContributionStatus.Proposed,
            CreatedAt = DateTime.UtcNow,
            VersionData = System.Text.Json.JsonSerializer.Serialize(updateDto),
            ChangeDescription = updateDto.ChangeDescription
        };

        var createdVersion = await _versionRepository.AddAsync(version);
        
        // Award points for contribution
        await _userService.AddUserActivityAsync(userId, ActivityType.EditVariety, 8, updateDto.Id);

        return _mapper.Map<TomatoVarietyVersionDto>(createdVersion);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        // Only allow deletion if user is admin or moderator
        var user = await _userService.GetProfileAsync(userId);
        if (user?.Role != UserRole.Administrator && user?.Role != UserRole.Moderator)
            return false;

        return await _varietyRepository.DeleteAsync(id);
    }

    public async Task<List<TomatoVarietyVersionDto>> GetVersionsAsync(int varietyId)
    {
        var versions = await _versionRepository.GetByVarietyIdAsync(varietyId);
        return _mapper.Map<List<TomatoVarietyVersionDto>>(versions);
    }

    public async Task<bool> ApproveVersionAsync(int versionId, string reviewerUserId)
    {
        var version = await _versionRepository.GetByIdAsync(versionId);
        if (version == null) return false;

        version.Status = ContributionStatus.Approved;
        version.ReviewedAt = DateTime.UtcNow;
        version.ReviewedByUserId = reviewerUserId;

        await _versionRepository.UpdateAsync(version);
          // Award points for having contribution approved
        await _userService.AddUserActivityAsync(version.UserId, ActivityType.Approve, 5, version.TomatoVarietyId);

        // Notify user of approval
        await _notificationService.NotifyVersionStatusChangeAsync(versionId, ContributionStatus.Approved);

        return true;
    }

    public async Task<bool> RejectVersionAsync(int versionId, string reviewerUserId, string reason)
    {
        var version = await _versionRepository.GetByIdAsync(versionId);
        if (version == null) return false;

        version.Status = ContributionStatus.Rejected;
        version.ReviewedAt = DateTime.UtcNow;
        version.ReviewedByUserId = reviewerUserId;
        version.RejectionReason = reason;

        await _versionRepository.UpdateAsync(version);

        // Notify user of rejection
        await _notificationService.NotifyVersionStatusChangeAsync(versionId, ContributionStatus.Rejected);

        return true;
    }

    public async Task<bool> FollowVarietyAsync(int varietyId, string userId)
    {
        var existingFollow = await _followRepository.GetAsync(userId, varietyId);
        if (existingFollow != null) return false;

        var follow = new UserFollowsVariety
        {
            UserId = userId,
            TomatoVarietyId = varietyId,
            FollowedAt = DateTime.UtcNow
        };

        await _followRepository.AddAsync(follow);
        return true;
    }

    public async Task<bool> UnfollowVarietyAsync(int varietyId, string userId)
    {
        return await _followRepository.RemoveAsync(userId, varietyId);
    }

    public async Task<List<TomatoVarietyDto>> GetFollowedVarietiesAsync(string userId)
    {
        var varieties = await _varietyRepository.GetFollowedByUserAsync(userId);
        return _mapper.Map<List<TomatoVarietyDto>>(varieties);
    }

    public async Task IncrementViewCountAsync(int varietyId)
    {
        await _varietyRepository.IncrementViewCountAsync(varietyId);
    }
}
