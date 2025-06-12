using Microsoft.EntityFrameworkCore;
using Tomatoz.Application.Interfaces;
using Tomatoz.Infrastructure.Data;
using Tomatoz.Shared.Enums;
using Tomatoz.Shared.Models;

namespace Tomatoz.Infrastructure.Repositories;

public class TomatoVarietyRepository : Repository<TomatoVariety>, ITomatoVarietyRepository
{
    public TomatoVarietyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<TomatoVariety>> SearchAsync(
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
        bool sortDescending = false)
    {
        var query = _dbSet.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(v => v.CommonName.Contains(searchTerm) ||
                                   (v.ScientificName != null && v.ScientificName.Contains(searchTerm)) ||
                                   (v.CultivationAdvice != null && v.CultivationAdvice.Contains(searchTerm)));
        }

        if (growthType.HasValue)
            query = query.Where(v => v.GrowthType == growthType.Value);

        if (color.HasValue)
            query = query.Where(v => v.Color == color.Value);

        if (shape.HasValue)
            query = query.Where(v => v.Shape == shape.Value);

        if (size.HasValue)
            query = query.Where(v => v.Size == size.Value);

        if (diseaseResistance.HasValue)
            query = query.Where(v => v.DiseaseResistance == diseaseResistance.Value);

        if (tags != null && tags.Any())
        {
            query = query.Where(v => v.Tags.Any(t => tags.Contains(t.Tag.Name)));
        }

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "name" => sortDescending ? query.OrderByDescending(v => v.CommonName) : query.OrderBy(v => v.CommonName),
            "rating" => sortDescending ? query.OrderByDescending(v => v.AverageRating) : query.OrderBy(v => v.AverageRating),
            "created" => sortDescending ? query.OrderByDescending(v => v.CreatedAt) : query.OrderBy(v => v.CreatedAt),
            "views" => sortDescending ? query.OrderByDescending(v => v.ViewCount) : query.OrderBy(v => v.ViewCount),
            _ => query.OrderBy(v => v.CommonName)
        };

        // Apply pagination
        return await query
            .Include(v => v.Photos.Where(p => p.IsApproved))
            .Include(v => v.Tags).ThenInclude(t => t.Tag)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetSearchCountAsync(
        string? searchTerm = null,
        TomatoGrowthType? growthType = null,
        TomatoColor? color = null,
        TomatoShape? shape = null,
        TomatoSize? size = null,
        DiseaseResistance? diseaseResistance = null,
        List<string>? tags = null)
    {
        var query = _dbSet.AsQueryable();

        // Apply same filters as search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(v => v.CommonName.Contains(searchTerm) ||
                                   (v.ScientificName != null && v.ScientificName.Contains(searchTerm)) ||
                                   (v.CultivationAdvice != null && v.CultivationAdvice.Contains(searchTerm)));
        }

        if (growthType.HasValue)
            query = query.Where(v => v.GrowthType == growthType.Value);

        if (color.HasValue)
            query = query.Where(v => v.Color == color.Value);

        if (shape.HasValue)
            query = query.Where(v => v.Shape == shape.Value);

        if (size.HasValue)
            query = query.Where(v => v.Size == size.Value);

        if (diseaseResistance.HasValue)
            query = query.Where(v => v.DiseaseResistance == diseaseResistance.Value);

        if (tags != null && tags.Any())
        {
            query = query.Where(v => v.Tags.Any(t => tags.Contains(t.Tag.Name)));
        }

        return await query.CountAsync();
    }

    public async Task<TomatoVariety?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(v => v.Photos.Where(p => p.IsApproved))
            .Include(v => v.Tags).ThenInclude(t => t.Tag)
            .Include(v => v.Votes)
            .Include(v => v.Comments.Where(c => c.IsApproved))
            .Include(v => v.Versions.Where(ver => ver.Status == ContributionStatus.Published))
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<TomatoVariety>> GetByUserIdAsync(string userId)
    {
        return await _context.TomatoVarietyVersions
            .Where(v => v.UserId == userId && v.Status == ContributionStatus.Published)
            .Select(v => v.TomatoVariety)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<TomatoVariety>> GetFollowedByUserAsync(string userId)
    {
        return await _context.UserFollowsVarieties
            .Where(f => f.UserId == userId)
            .Select(f => f.TomatoVariety)
            .Include(v => v.Photos.Where(p => p.IsApproved))
            .Include(v => v.Tags).ThenInclude(t => t.Tag)
            .ToListAsync();
    }

    public async Task IncrementViewCountAsync(int id)
    {
        var variety = await _dbSet.FindAsync(id);
        if (variety != null)
        {
            variety.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }
}

public class TomatoVarietyVersionRepository : Repository<TomatoVarietyVersion>, ITomatoVarietyVersionRepository
{
    public TomatoVarietyVersionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<TomatoVarietyVersion>> GetByVarietyIdAsync(int varietyId)
    {
        return await _dbSet
            .Where(v => v.TomatoVarietyId == varietyId)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync();
    }

    public async Task<List<TomatoVarietyVersion>> GetPendingVersionsAsync()
    {
        return await _dbSet
            .Where(v => v.Status == ContributionStatus.Proposed)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task<TomatoVarietyVersion?> GetLatestApprovedVersionAsync(int varietyId)
    {
        return await _dbSet
            .Where(v => v.TomatoVarietyId == varietyId && v.Status == ContributionStatus.Approved)
            .OrderByDescending(v => v.VersionNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<List<TomatoVarietyVersion>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(v => v.UserId == userId)
            .Include(v => v.TomatoVariety)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }
}

public class TomatoPhotoRepository : Repository<TomatoPhoto>, ITomatoPhotoRepository
{
    public TomatoPhotoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<TomatoPhoto>> GetByVarietyIdAsync(int varietyId)
    {
        return await _dbSet
            .Where(p => p.TomatoVarietyId == varietyId)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.UploadedAt)
            .ToListAsync();
    }

    public async Task<List<TomatoPhoto>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(p => p.UserId == userId)
            .Include(p => p.TomatoVariety)
            .OrderByDescending(p => p.UploadedAt)
            .ToListAsync();
    }

    public async Task<List<TomatoPhoto>> GetPendingPhotosAsync()
    {
        return await _dbSet
            .Where(p => !p.IsApproved)
            .Include(p => p.TomatoVariety)
            .OrderBy(p => p.UploadedAt)
            .ToListAsync();
    }

    public async Task<List<TomatoPhoto>> GetApprovedPhotosByVarietyAsync(int varietyId)
    {
        return await _dbSet
            .Where(p => p.TomatoVarietyId == varietyId && p.IsApproved)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.UploadedAt)
            .ToListAsync();
    }
}

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<List<Tag>> SearchAsync(string query)
    {
        return await _dbSet
            .Where(t => t.Name.Contains(query) || (t.Description != null && t.Description.Contains(query)))
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetPopularTagsAsync(int count)
    {
        return await _dbSet
            .OrderByDescending(t => t.UsageCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetByVarietyIdAsync(int varietyId)
    {
        return await _context.TomatoVarietyTags
            .Where(vt => vt.TomatoVarietyId == varietyId)
            .Select(vt => vt.Tag)
            .ToListAsync();
    }
}

public class VoteRepository : Repository<Vote>, IVoteRepository
{
    public VoteRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Vote?> GetUserVoteAsync(int varietyId, string userId, VoteType voteType)
    {
        return await _dbSet
            .FirstOrDefaultAsync(v => v.TomatoVarietyId == varietyId && v.UserId == userId && v.VoteType == voteType);
    }

    public async Task<List<Vote>> GetByVarietyIdAsync(int varietyId)
    {
        return await _dbSet
            .Where(v => v.TomatoVarietyId == varietyId)
            .ToListAsync();
    }

    public async Task<List<Vote>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(v => v.UserId == userId)
            .Include(v => v.TomatoVariety)
            .ToListAsync();
    }

    public async Task<Dictionary<VoteType, decimal>> GetAverageRatingsAsync(int varietyId)
    {
        var votes = await _dbSet
            .Where(v => v.TomatoVarietyId == varietyId)
            .GroupBy(v => v.VoteType)            .Select(g => new { VoteType = g.Key, Average = g.Average(v => v.Rating) })
            .ToListAsync();

        return votes.ToDictionary(v => v.VoteType, v => (decimal)v.Average);
    }
}

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Comment>> GetByVarietyIdAsync(int varietyId)
    {
        return await _dbSet
            .Where(c => c.TomatoVarietyId == varietyId)
            .Include(c => c.Replies)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId)
            .Include(c => c.TomatoVariety)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetPendingCommentsAsync()
    {
        return await _dbSet
            .Where(c => !c.IsApproved)
            .Include(c => c.TomatoVariety)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetApprovedCommentsByVarietyAsync(int varietyId)
    {
        return await _dbSet
            .Where(c => c.TomatoVarietyId == varietyId && c.IsApproved)
            .Include(c => c.Replies.Where(r => r.IsApproved))
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }
}

public class UserActivityRepository : Repository<UserActivity>, IUserActivityRepository
{
    public UserActivityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<UserActivity>> GetByUserIdAsync(string userId, int page = 1, int pageSize = 20)
    {
        return await _dbSet
            .Where(a => a.UserId == userId)
            .Include(a => a.TomatoVariety)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetUserPointsAsync(string userId)
    {
        return await _dbSet
            .Where(a => a.UserId == userId)
            .SumAsync(a => a.PointsEarned);
    }

    public async Task<List<UserActivity>> GetRecentActivitiesAsync(int count = 50)
    {
        return await _dbSet
            .Include(a => a.TomatoVariety)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}

public class UserFollowsVarietyRepository : IUserFollowsVarietyRepository
{
    private readonly ApplicationDbContext _context;

    public UserFollowsVarietyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserFollowsVariety?> GetAsync(string userId, int varietyId)
    {
        return await _context.UserFollowsVarieties
            .FirstOrDefaultAsync(f => f.UserId == userId && f.TomatoVarietyId == varietyId);
    }

    public async Task<UserFollowsVariety> AddAsync(UserFollowsVariety follow)
    {
        await _context.UserFollowsVarieties.AddAsync(follow);
        await _context.SaveChangesAsync();
        return follow;
    }

    public async Task<bool> RemoveAsync(string userId, int varietyId)
    {
        var follow = await GetAsync(userId, varietyId);
        if (follow == null) return false;

        _context.UserFollowsVarieties.Remove(follow);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserFollowsVariety>> GetByUserIdAsync(string userId)
    {
        return await _context.UserFollowsVarieties
            .Where(f => f.UserId == userId)
            .Include(f => f.TomatoVariety)
            .ToListAsync();
    }

    public async Task<List<UserFollowsVariety>> GetByVarietyIdAsync(int varietyId)
    {
        return await _context.UserFollowsVarieties
            .Where(f => f.TomatoVarietyId == varietyId)
            .ToListAsync();
    }

    public async Task<bool> IsFollowingAsync(string userId, int varietyId)
    {
        return await _context.UserFollowsVarieties
            .AnyAsync(f => f.UserId == userId && f.TomatoVarietyId == varietyId);
    }
}
