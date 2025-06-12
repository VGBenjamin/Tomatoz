using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Models;
using Tomatoz.Shared.Enums;

namespace Tomatoz.Application.Interfaces;

public interface ITomatoVarietyService
{
    Task<PaginatedResultDto<TomatoVarietyDto>> SearchAsync(TomatoVarietySearchDto searchDto);
    Task<TomatoVarietyDto?> GetByIdAsync(int id);
    Task<TomatoVarietyDto> CreateAsync(CreateTomatoVarietyDto createDto, string userId);
    Task<TomatoVarietyVersionDto> ProposeUpdateAsync(UpdateTomatoVarietyDto updateDto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<List<TomatoVarietyVersionDto>> GetVersionsAsync(int varietyId);
    Task<bool> ApproveVersionAsync(int versionId, string reviewerUserId);
    Task<bool> RejectVersionAsync(int versionId, string reviewerUserId, string reason);
    Task<bool> FollowVarietyAsync(int varietyId, string userId);
    Task<bool> UnfollowVarietyAsync(int varietyId, string userId);
    Task<List<TomatoVarietyDto>> GetFollowedVarietiesAsync(string userId);
    Task IncrementViewCountAsync(int varietyId);
}

public interface ITomatoPhotoService
{
    Task<List<TomatoPhotoDto>> GetByVarietyIdAsync(int varietyId);
    Task<TomatoPhotoDto> UploadAsync(UploadPhotoDto uploadDto, Stream fileStream, string fileName, string userId);
    Task<bool> DeleteAsync(int photoId, string userId);
    Task<bool> ApproveAsync(int photoId, string moderatorUserId);
    Task<bool> UpdateOrderAsync(int photoId, int newOrder, string userId);
}

public interface IVoteService
{
    Task<VoteDto?> GetUserVoteAsync(int varietyId, string userId, VoteType voteType);
    Task<VoteDto> CreateOrUpdateVoteAsync(CreateVoteDto voteDto, string userId);
    Task<bool> DeleteVoteAsync(int varietyId, string userId, VoteType voteType);
    Task<Dictionary<VoteType, decimal>> GetAverageRatingsAsync(int varietyId);
}

public interface ICommentService
{
    Task<List<CommentDto>> GetByVarietyIdAsync(int varietyId);
    Task<CommentDto> CreateAsync(CreateCommentDto commentDto, string userId);
    Task<bool> UpdateAsync(int commentId, string content, string userId);
    Task<bool> DeleteAsync(int commentId, string userId);
    Task<bool> ApproveAsync(int commentId, string moderatorUserId);
}

public interface ITagService
{
    Task<List<TagDto>> GetAllAsync();
    Task<List<TagDto>> SearchAsync(string query);
    Task<TagDto> CreateAsync(string name, string? description, string userId);
    Task<List<TagDto>> GetPopularTagsAsync(int count = 20);
}

public interface IUserService
{
    Task<UserProfileDto?> GetProfileAsync(string userId);
    Task<bool> UpdateProfileAsync(string userId, string? displayName, string? bio);
    Task<List<UserActivityDto>> GetUserActivitiesAsync(string userId, int page = 1, int pageSize = 20);
    Task<int> GetUserPointsAsync(string userId);
    Task AddUserActivityAsync(string userId, ActivityType activityType, int pointsEarned, int? varietyId = null, string? description = null);
    Task<List<UserProfileDto>> GetTopContributorsAsync(int count = 10);
}

public interface IModerationService
{
    Task<List<TomatoVarietyVersionDto>> GetPendingVersionsAsync();
    Task<List<CommentDto>> GetPendingCommentsAsync();
    Task<List<TomatoPhotoDto>> GetPendingPhotosAsync();
    Task<bool> BulkApproveVersionsAsync(List<int> versionIds, string reviewerUserId);
    Task<bool> BulkRejectVersionsAsync(List<int> versionIds, string reviewerUserId, string reason);
}

public interface INotificationService
{
    Task NotifyVersionStatusChangeAsync(int versionId, ContributionStatus newStatus);
    Task NotifyNewCommentAsync(int varietyId, string commenterUserId);
    Task NotifyVarietyUpdateAsync(int varietyId, List<string> followerUserIds);
}

public interface ISearchService
{
    Task<List<TomatoVarietyDto>> FullTextSearchAsync(string query, int maxResults = 50);
    Task<List<TagDto>> SearchTagsAsync(string query, int maxResults = 20);
    Task RebuildSearchIndexAsync();
}
