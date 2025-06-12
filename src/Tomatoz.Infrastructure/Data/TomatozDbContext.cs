using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tomatoz.Shared.Models;
using Tomatoz.Shared.Enums;

namespace Tomatoz.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TomatoVariety> TomatoVarieties { get; set; } = null!;
    public DbSet<TomatoVarietyVersion> TomatoVarietyVersions { get; set; } = null!;
    public DbSet<TomatoPhoto> TomatoPhotos { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<TomatoVarietyTag> TomatoVarietyTags { get; set; } = null!;
    public DbSet<Vote> Votes { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<UserActivity> UserActivities { get; set; } = null!;
    public DbSet<UserFollowsVariety> UserFollowsVarieties { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);        // Configure entity relationships and constraints
        modelBuilder.Entity<TomatoVariety>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CommonName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ScientificName).HasMaxLength(300);
            entity.Property(e => e.CultivationAdvice).HasMaxLength(2000);
            entity.Property(e => e.HistoryAndOrigin).HasMaxLength(2000);
            entity.HasIndex(e => e.CommonName).IsUnique();
            
            // Configure decimal precision
            entity.Property(e => e.AverageRating).HasPrecision(3, 2);
            entity.Property(e => e.MinSizeCm).HasPrecision(5, 2);
            entity.Property(e => e.MaxSizeCm).HasPrecision(5, 2);
            entity.Property(e => e.YieldPerPlantKg).HasPrecision(5, 2);
            
            // Configure relationships
            entity.HasMany(e => e.Versions)
                  .WithOne(v => v.TomatoVariety)
                  .HasForeignKey(v => v.TomatoVarietyId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasMany(e => e.Photos)
                  .WithOne(p => p.TomatoVariety)
                  .HasForeignKey(p => p.TomatoVarietyId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasMany(e => e.Tags)
                  .WithOne(t => t.TomatoVariety)
                  .HasForeignKey(t => t.TomatoVarietyId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasMany(e => e.Votes)
                  .WithOne(v => v.TomatoVariety)
                  .HasForeignKey(v => v.TomatoVarietyId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasMany(e => e.Comments)
                  .WithOne(c => c.TomatoVariety)
                  .HasForeignKey(c => c.TomatoVarietyId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasMany(e => e.Followers)
                  .WithOne(f => f.TomatoVariety)
                  .HasForeignKey(f => f.TomatoVarietyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });        modelBuilder.Entity<TomatoVarietyVersion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VersionData).IsRequired().HasMaxLength(8000);
            entity.Property(e => e.RejectionReason).HasMaxLength(1000);
            entity.Property(e => e.ChangeDescription).HasMaxLength(2000);
            
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(e => e.ReviewedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.ReviewedByUserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TomatoPhoto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
        });        modelBuilder.Entity<TomatoVarietyTag>(entity =>
        {
            entity.HasKey(e => new { e.TomatoVarietyId, e.TagId });
            
            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.TomatoVarieties)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.AddedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.AddedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // Composite unique index to prevent duplicate votes
            entity.HasIndex(e => new { e.TomatoVarietyId, e.UserId, e.VoteType }).IsUnique();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.UserId).IsRequired();
            
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.ActivityType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserFollowsVariety>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TomatoVarietyId });
            
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });        // Seed data
        SeedData(modelBuilder);
    }    private void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Heritage", Description = "Traditional heirloom varieties", CreatedAt = seedDate, UsageCount = 0 },
            new Tag { Id = 2, Name = "Disease Resistant", Description = "Varieties with good disease resistance", CreatedAt = seedDate, UsageCount = 0 },
            new Tag { Id = 3, Name = "High Yield", Description = "Very productive varieties", CreatedAt = seedDate, UsageCount = 0 },
            new Tag { Id = 4, Name = "Container Friendly", Description = "Suitable for container growing", CreatedAt = seedDate, UsageCount = 0 },
            new Tag { Id = 5, Name = "Cold Tolerant", Description = "Tolerates cooler temperatures", CreatedAt = seedDate, UsageCount = 0 }
        );

        modelBuilder.Entity<TomatoVariety>().HasData(
            new TomatoVariety
            {
                Id = 1,
                CommonName = "San Marzano",
                ScientificName = "Solanum lycopersicum",
                GrowthType = TomatoGrowthType.Indeterminate,
                AverageHeightCm = 180,
                Shape = TomatoShape.Roma,
                Color = TomatoColor.Red,
                Size = TomatoSize.Medium,
                MinWeightGrams = 60,
                MaxWeightGrams = 100,
                MinSizeCm = 6,
                MaxSizeCm = 8,
                ProductivityRating = 8,
                DaysToMaturity = 80,
                YieldPerPlantKg = 4.5m,
                FleshType = TomatoFleshType.Dense,
                Texture = TomatoTexture.Firm,
                DiseaseResistance = DiseaseResistance.Moderate,
                CultivationAdvice = "Classic Italian paste tomato. Requires support and regular pruning.",
                HistoryAndOrigin = "Originated in the volcanic soil near Naples, Italy.",
                CreatedAt = seedDate,
                AverageRating = 4.2m,
                ViewCount = 0
            },            new TomatoVariety
            {
                Id = 2,
                CommonName = "Cherokee Purple",
                ScientificName = "Solanum lycopersicum",
                GrowthType = TomatoGrowthType.Indeterminate,
                AverageHeightCm = 200,
                Shape = TomatoShape.Beefsteak,
                Color = TomatoColor.Purple,
                Size = TomatoSize.Large,
                MinWeightGrams = 200,
                MaxWeightGrams = 400,
                MinSizeCm = 8,
                MaxSizeCm = 12,
                ProductivityRating = 7,
                DaysToMaturity = 90,
                YieldPerPlantKg = 3.5m,
                FleshType = TomatoFleshType.Regular,
                Texture = TomatoTexture.Juicy,
                DiseaseResistance = DiseaseResistance.Low,
                CultivationAdvice = "Beautiful purple heirloom requiring warm conditions and good support.",
                HistoryAndOrigin = "Heirloom variety believed to originate from Cherokee Nation in Tennessee.",
                CreatedAt = seedDate,
                AverageRating = 4.5m,
                ViewCount = 0
            }
        );
    }
}
