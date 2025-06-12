using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Enums;
using Tomatoz.Shared.Models;

namespace Tomatoz.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface ITomatoVarietyRepository : IRepository<TomatoVariety>
{
    Task<List<TomatoVariety>> SearchAsync(
        string? searchTerm = null,
        TomatoGrowthType? growthType = null,
        TomatoColor? color = null,
        TomatoShape? shape = null,
        TomatoSize? size = null,
        DiseaseResistance? diseaseResistance = null,
        List<string>? tags = null,
        int page = 1,
        int pageSize = 20,
        string sortBy = "Name",
        bool sortDescending = false);
    
    Task<int> GetSearchCountAsync(
        string? searchTerm = null,
        TomatoGrowthType? growthType = null,
        TomatoColor? color = null,
        TomatoShape? shape = null,
        TomatoSize? size = null,
        DiseaseResistance? diseaseResistance = null,
        List<string>? tags = null);
    
    Task<TomatoVariety?> GetWithDetailsAsync(int id);
    Task<List<TomatoVariety>> GetByUserIdAsync(string userId);
    Task<List<TomatoVariety>> GetFollowedByUserAsync(string userId);
    Task IncrementViewCountAsync(int id);
}

public interface ITomatoVarietyVersionRepository : IRepository<TomatoVarietyVersion>
{
    Task<List<TomatoVarietyVersion>> GetByVarietyIdAsync(int varietyId);
    Task<List<TomatoVarietyVersion>> GetPendingVersionsAsync();
    Task<TomatoVarietyVersion?> GetLatestApprovedVersionAsync(int varietyId);
    Task<List<TomatoVarietyVersion>> GetByUserIdAsync(string userId);
}

public interface ITomatoPhotoRepository : IRepository<TomatoPhoto>
{
    Task<List<TomatoPhoto>> GetByVarietyIdAsync(int varietyId);
    Task<List<TomatoPhoto>> GetByUserIdAsync(string userId);
    Task<List<TomatoPhoto>> GetPendingPhotosAsync();
    Task<List<TomatoPhoto>> GetApprovedPhotosByVarietyAsync(int varietyId);
}

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name);
    Task<List<Tag>> SearchAsync(string query);
    Task<List<Tag>> GetPopularTagsAsync(int count);
    Task<List<Tag>> GetByVarietyIdAsync(int varietyId);
}

public interface IVoteRepository : IRepository<Vote>
{
    Task<Vote?> GetUserVoteAsync(int varietyId, string userId, VoteType voteType);
    Task<List<Vote>> GetByVarietyIdAsync(int varietyId);
    Task<List<Vote>> GetByUserIdAsync(string userId);
    Task<Dictionary<VoteType, decimal>> GetAverageRatingsAsync(int varietyId);
}

public interface ICommentRepository : IRepository<Comment>
{
    Task<List<Comment>> GetByVarietyIdAsync(int varietyId);
    Task<List<Comment>> GetByUserIdAsync(string userId);
    Task<List<Comment>> GetPendingCommentsAsync();
    Task<List<Comment>> GetApprovedCommentsByVarietyAsync(int varietyId);
}

public interface IUserActivityRepository : IRepository<UserActivity>
{
    Task<List<UserActivity>> GetByUserIdAsync(string userId, int page = 1, int pageSize = 20);
    Task<int> GetUserPointsAsync(string userId);
    Task<List<UserActivity>> GetRecentActivitiesAsync(int count = 50);
}

public interface IUserFollowsVarietyRepository
{
    Task<UserFollowsVariety?> GetAsync(string userId, int varietyId);
    Task<UserFollowsVariety> AddAsync(UserFollowsVariety follow);
    Task<bool> RemoveAsync(string userId, int varietyId);
    Task<List<UserFollowsVariety>> GetByUserIdAsync(string userId);
    Task<List<UserFollowsVariety>> GetByVarietyIdAsync(int varietyId);
    Task<bool> IsFollowingAsync(string userId, int varietyId);
}
