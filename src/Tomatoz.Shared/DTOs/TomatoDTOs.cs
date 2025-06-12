using System.ComponentModel.DataAnnotations;
using Tomatoz.Shared.Enums;

namespace Tomatoz.Shared.DTOs;

public class TomatoVarietyDto
{
    public int Id { get; set; }
    public string CommonName { get; set; } = string.Empty;
    public string? ScientificName { get; set; }
    public TomatoGrowthType GrowthType { get; set; }
    public int? AverageHeightCm { get; set; }
    public TomatoShape Shape { get; set; }
    public TomatoColor Color { get; set; }
    public TomatoSize Size { get; set; }
    public int? MinWeightGrams { get; set; }
    public int? MaxWeightGrams { get; set; }
    public decimal? MinSizeCm { get; set; }
    public decimal? MaxSizeCm { get; set; }
    public int? ProductivityRating { get; set; }
    public int? DaysToMaturity { get; set; }
    public decimal? YieldPerPlantKg { get; set; }
    public TomatoFleshType FleshType { get; set; }
    public TomatoTexture Texture { get; set; }
    public DiseaseResistance DiseaseResistance { get; set; }
    public string? CultivationAdvice { get; set; }
    public string? HistoryAndOrigin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal AverageRating { get; set; }
    public int ViewCount { get; set; }
    public List<TomatoPhotoDto> Photos { get; set; } = new();
    public List<TagDto> Tags { get; set; } = new();
    public List<string> ContributorNames { get; set; } = new();
}

public class CreateTomatoVarietyDto
{
    [Required, MaxLength(200)]
    public string CommonName { get; set; } = string.Empty;
    
    [MaxLength(300)]
    public string? ScientificName { get; set; }
    
    public TomatoGrowthType GrowthType { get; set; }
    
    [Range(0, 500)]
    public int? AverageHeightCm { get; set; }
    
    public TomatoShape Shape { get; set; }
    public TomatoColor Color { get; set; }
    public TomatoSize Size { get; set; }
    
    [Range(0, 5000)]
    public int? MinWeightGrams { get; set; }
    
    [Range(0, 5000)]
    public int? MaxWeightGrams { get; set; }
    
    [Range(0, 20)]
    public decimal? MinSizeCm { get; set; }
    
    [Range(0, 20)]
    public decimal? MaxSizeCm { get; set; }
    
    [Range(0, 10)]
    public int? ProductivityRating { get; set; }
    
    [Range(0, 200)]
    public int? DaysToMaturity { get; set; }
    
    [Range(0, 100)]
    public decimal? YieldPerPlantKg { get; set; }
    
    public TomatoFleshType FleshType { get; set; }
    public TomatoTexture Texture { get; set; }
    public DiseaseResistance DiseaseResistance { get; set; }
    
    [MaxLength(2000)]
    public string? CultivationAdvice { get; set; }
    
    [MaxLength(2000)]
    public string? HistoryAndOrigin { get; set; }
    
    public List<string> Tags { get; set; } = new();
}

public class UpdateTomatoVarietyDto
{
    public int Id { get; set; }
    
    [Required, MaxLength(200)]
    public string CommonName { get; set; } = string.Empty;
    
    [MaxLength(300)]
    public string? ScientificName { get; set; }
    
    public TomatoGrowthType GrowthType { get; set; }
    
    [Range(0, 500)]
    public int? AverageHeightCm { get; set; }
    
    public TomatoShape Shape { get; set; }
    public TomatoColor Color { get; set; }
    public TomatoSize Size { get; set; }
    
    [Range(0, 5000)]
    public int? MinWeightGrams { get; set; }
    
    [Range(0, 5000)]
    public int? MaxWeightGrams { get; set; }
    
    [Range(0, 20)]
    public decimal? MinSizeCm { get; set; }
    
    [Range(0, 20)]
    public decimal? MaxSizeCm { get; set; }
    
    [Range(0, 10)]
    public int? ProductivityRating { get; set; }
    
    [Range(0, 200)]
    public int? DaysToMaturity { get; set; }
    
    [Range(0, 100)]
    public decimal? YieldPerPlantKg { get; set; }
    
    public TomatoFleshType FleshType { get; set; }
    public TomatoTexture Texture { get; set; }
    public DiseaseResistance DiseaseResistance { get; set; }
    
    [MaxLength(2000)]
    public string? CultivationAdvice { get; set; }
    
    [MaxLength(2000)]
    public string? HistoryAndOrigin { get; set; }
    
    [MaxLength(2000)]
    public string? ChangeDescription { get; set; }
    
    public List<string> Tags { get; set; } = new();
}

public class TomatoPhotoDto
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public PhotoType PhotoType { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool IsApproved { get; set; }
    public int DisplayOrder { get; set; }
    public string UploaderName { get; set; } = string.Empty;
}

public class UploadPhotoDto
{
    public int TomatoVarietyId { get; set; }
    
    [MaxLength(200)]
    public string? Title { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public PhotoType PhotoType { get; set; }
    
    [Range(1, 10)]
    public int DisplayOrder { get; set; } = 1;
}

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int UsageCount { get; set; }
}

public class VoteDto
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public VoteType VoteType { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateVoteDto
{
    public int TomatoVarietyId { get; set; }
    public VoteType VoteType { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsApproved { get; set; }
    public int? ParentCommentId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public List<CommentDto> Replies { get; set; } = new();
}

public class CreateCommentDto
{
    public int TomatoVarietyId { get; set; }
    
    [Required, MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
    
    public int? ParentCommentId { get; set; }
}

public class UserActivityDto
{
    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public int? TomatoVarietyId { get; set; }
    public string? TomatoVarietyName { get; set; }
    public int PointsEarned { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Description { get; set; }
}

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public UserRole Role { get; set; }
    public int TotalPoints { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastActiveAt { get; set; }
    public string? Bio { get; set; }
    public int ContributionsCount { get; set; }
    public int PhotosCount { get; set; }
    public List<UserActivityDto> RecentActivities { get; set; } = new();
}

public class TomatoVarietyVersionDto
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int VersionNumber { get; set; }
    public ContributionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewedByUserName { get; set; }
    public string? RejectionReason { get; set; }
    public string? ChangeDescription { get; set; }
}

public class TomatoVarietySearchDto
{
    public string? SearchTerm { get; set; }
    public TomatoGrowthType? GrowthType { get; set; }
    public TomatoColor? Color { get; set; }
    public TomatoShape? Shape { get; set; }
    public TomatoSize? Size { get; set; }
    public DiseaseResistance? DiseaseResistance { get; set; }
    public List<string> Tags { get; set; } = new();
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "Name";
    public bool SortDescending { get; set; } = false;
}

public class PaginatedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
