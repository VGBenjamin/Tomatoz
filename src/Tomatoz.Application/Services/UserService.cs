using AutoMapper;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Enums;
using Tomatoz.Shared.Models;

namespace Tomatoz.Application.Services;

public class UserService : IUserService
{
    private readonly IUserActivityRepository _activityRepository;
    private readonly IMapper _mapper;

    public UserService(IUserActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<UserProfileDto?> GetProfileAsync(string userId)
    {
        // This would typically get user from Identity system
        // For now, return a basic profile
        return new UserProfileDto
        {
            Id = userId,
            Role = UserRole.User, // Default role
            TotalPoints = await GetUserPointsAsync(userId),
            CreatedAt = DateTime.UtcNow.AddDays(-30), // Placeholder
            RecentActivities = await GetUserActivitiesAsync(userId, 1, 5)
        };
    }

    public async Task<bool> UpdateProfileAsync(string userId, string? displayName, string? bio)
    {
        // Implementation would update user profile in Identity system
        return true;
    }

    public async Task<List<UserActivityDto>> GetUserActivitiesAsync(string userId, int page = 1, int pageSize = 20)
    {
        var activities = await _activityRepository.GetByUserIdAsync(userId, page, pageSize);
        return _mapper.Map<List<UserActivityDto>>(activities);
    }

    public async Task<int> GetUserPointsAsync(string userId)
    {
        return await _activityRepository.GetUserPointsAsync(userId);
    }

    public async Task AddUserActivityAsync(string userId, ActivityType activityType, int pointsEarned, int? varietyId = null, string? description = null)
    {
        var activity = new UserActivity
        {
            UserId = userId,
            ActivityType = activityType,
            TomatoVarietyId = varietyId,
            PointsEarned = pointsEarned,
            CreatedAt = DateTime.UtcNow,
            Description = description
        };

        await _activityRepository.AddAsync(activity);
    }

    public async Task<List<UserProfileDto>> GetTopContributorsAsync(int count = 10)
    {
        // Implementation would get top contributors by points
        return new List<UserProfileDto>();
    }
}
