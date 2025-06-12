using System.ComponentModel.DataAnnotations;
using Tomatoz.Shared.Enums;

namespace Tomatoz.Shared.Models;

public class TomatoVariety
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
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    [Range(0, 5)]
    public decimal AverageRating { get; set; }
    
    public int ViewCount { get; set; }
    
    // Navigation properties
    public virtual ICollection<TomatoVarietyVersion> Versions { get; set; } = new List<TomatoVarietyVersion>();
    public virtual ICollection<TomatoPhoto> Photos { get; set; } = new List<TomatoPhoto>();
    public virtual ICollection<TomatoVarietyTag> Tags { get; set; } = new List<TomatoVarietyTag>();
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<UserFollowsVariety> Followers { get; set; } = new List<UserFollowsVariety>();
}

public class TomatoVarietyVersion
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    public int VersionNumber { get; set; }
    public ContributionStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewedByUserId { get; set; }
    
    [MaxLength(1000)]
    public string? RejectionReason { get; set; }
    
    [MaxLength(2000)]
    public string? ChangeDescription { get; set; }
    
    // JSON field containing the version data
    [MaxLength(8000)]
    public string VersionData { get; set; } = string.Empty;
      // Navigation properties
    public virtual TomatoVariety TomatoVariety { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ApplicationUser? ReviewedByUser { get; set; }
}

public class TomatoPhoto
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    [Required, MaxLength(500)]
    public string FileName { get; set; } = string.Empty;
    
    [Required, MaxLength(1000)]
    public string FilePath { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Title { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public PhotoType PhotoType { get; set; }
    
    public DateTime UploadedAt { get; set; }
    public bool IsApproved { get; set; }
    
    [Range(1, 10)]
    public int DisplayOrder { get; set; }
    
    public long FileSizeBytes { get; set; }
    
    // Navigation properties
    public virtual TomatoVariety TomatoVariety { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}

public class Tag
{
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public int UsageCount { get; set; }
    
    // Navigation properties
    public virtual ICollection<TomatoVarietyTag> TomatoVarieties { get; set; } = new List<TomatoVarietyTag>();
}

public class TomatoVarietyTag
{
    public int TomatoVarietyId { get; set; }
    public int TagId { get; set; }
    
    public DateTime AddedAt { get; set; }
    public string AddedByUserId { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual TomatoVariety TomatoVariety { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
    public virtual ApplicationUser AddedByUser { get; set; } = null!;
}

public class Vote
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    public VoteType VoteType { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual TomatoVariety TomatoVariety { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}

public class Comment
{
    public int Id { get; set; }
    public int TomatoVarietyId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    [Required, MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsApproved { get; set; }
    
    public int? ParentCommentId { get; set; }
    
    // Navigation properties
    public virtual TomatoVariety TomatoVariety { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
}

public class UserActivity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    public ActivityType ActivityType { get; set; }
    public int? TomatoVarietyId { get; set; }
    
    [Range(1, 10)]
    public int PointsEarned { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual TomatoVariety? TomatoVariety { get; set; }
}

public class UserFollowsVariety
{
    public string UserId { get; set; } = string.Empty;
    public int TomatoVarietyId { get; set; }
    
    public DateTime FollowedAt { get; set; }
      // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual TomatoVariety TomatoVariety { get; set; } = null!;
}
