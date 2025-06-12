using AutoMapper;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Enums;
using Tomatoz.Shared.Models;

namespace Tomatoz.Application.Services;

public class ModerationService : IModerationService
{
    private readonly ITomatoVarietyVersionRepository _versionRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ITomatoPhotoRepository _photoRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public ModerationService(
        ITomatoVarietyVersionRepository versionRepository,
        ICommentRepository commentRepository,
        ITomatoPhotoRepository photoRepository,
        IUserService userService,
        IMapper mapper)
    {
        _versionRepository = versionRepository;
        _commentRepository = commentRepository;
        _photoRepository = photoRepository;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<List<TomatoVarietyVersionDto>> GetPendingVersionsAsync()
    {
        var versions = await _versionRepository.GetPendingVersionsAsync();
        return _mapper.Map<List<TomatoVarietyVersionDto>>(versions);
    }

    public async Task<List<CommentDto>> GetPendingCommentsAsync()
    {
        var comments = await _commentRepository.GetPendingCommentsAsync();
        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<List<TomatoPhotoDto>> GetPendingPhotosAsync()
    {
        var photos = await _photoRepository.GetPendingPhotosAsync();
        return _mapper.Map<List<TomatoPhotoDto>>(photos);
    }

    public async Task<bool> BulkApproveVersionsAsync(List<int> versionIds, string reviewerUserId)
    {
        foreach (var versionId in versionIds)
        {
            var version = await _versionRepository.GetByIdAsync(versionId);
            if (version != null)
            {
                version.Status = ContributionStatus.Approved;
                version.ReviewedAt = DateTime.UtcNow;
                version.ReviewedByUserId = reviewerUserId;
                await _versionRepository.UpdateAsync(version);

                // Add points to contributor
                await _userService.AddUserActivityAsync(version.UserId, ActivityType.Approve, 8, version.TomatoVarietyId, "Version approved");
            }
        }
        return true;
    }

    public async Task<bool> BulkRejectVersionsAsync(List<int> versionIds, string reviewerUserId, string reason)
    {
        foreach (var versionId in versionIds)
        {
            var version = await _versionRepository.GetByIdAsync(versionId);
            if (version != null)
            {
                version.Status = ContributionStatus.Rejected;
                version.ReviewedAt = DateTime.UtcNow;
                version.ReviewedByUserId = reviewerUserId;
                version.RejectionReason = reason;
                await _versionRepository.UpdateAsync(version);
            }
        }
        return true;
    }
}

public class NotificationService : INotificationService
{
    private readonly IUserFollowsVarietyRepository _followRepository;
    private readonly ITomatoVarietyRepository _varietyRepository;

    public NotificationService(
        IUserFollowsVarietyRepository followRepository,
        ITomatoVarietyRepository varietyRepository)
    {
        _followRepository = followRepository;
        _varietyRepository = varietyRepository;
    }

    public async Task NotifyVersionStatusChangeAsync(int versionId, ContributionStatus newStatus)
    {
        // Implementation would send email/push notifications
        // For now, this is a placeholder
        await Task.CompletedTask;
    }

    public async Task NotifyNewCommentAsync(int varietyId, string commenterUserId)
    {
        var followers = await _followRepository.GetByVarietyIdAsync(varietyId);
        
        // Send notifications to all followers except the commenter
        foreach (var follower in followers.Where(f => f.UserId != commenterUserId))
        {
            // Implementation would send email/push notifications
            // For now, this is a placeholder
        }
        
        await Task.CompletedTask;
    }

    public async Task NotifyVarietyUpdateAsync(int varietyId, List<string> followerUserIds)
    {
        // Implementation would send email/push notifications to followers
        // For now, this is a placeholder
        await Task.CompletedTask;
    }
}

public class SearchService : ISearchService
{
    private readonly ITomatoVarietyRepository _varietyRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public SearchService(
        ITomatoVarietyRepository varietyRepository,
        ITagRepository tagRepository,
        IMapper mapper)
    {
        _varietyRepository = varietyRepository;
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<List<TomatoVarietyDto>> FullTextSearchAsync(string query, int maxResults = 50)
    {
        var varieties = await _varietyRepository.SearchAsync(
            searchTerm: query,
            pageSize: maxResults,
            sortBy: "rating",
            sortDescending: true);

        return _mapper.Map<List<TomatoVarietyDto>>(varieties);
    }

    public async Task<List<TagDto>> SearchTagsAsync(string query, int maxResults = 20)
    {
        var tags = await _tagRepository.SearchAsync(query);
        var limitedTags = tags.Take(maxResults).ToList();
        return _mapper.Map<List<TagDto>>(limitedTags);
    }

    public async Task RebuildSearchIndexAsync()
    {
        // Implementation would rebuild search indexes
        // For now, this is a placeholder
        await Task.CompletedTask;
    }
}
