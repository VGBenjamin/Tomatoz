using AutoMapper;
using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Models;

namespace Tomatoz.Application.Mapping;

public class TomatoMappingProfile : Profile
{
    public TomatoMappingProfile()
    {
        // TomatoVariety mappings
        CreateMap<TomatoVariety, TomatoVarietyDto>()
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Where(p => p.IsApproved)))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)))
            .ForMember(dest => dest.ContributorNames, opt => opt.Ignore()); // Will be populated separately

        CreateMap<CreateTomatoVarietyDto, TomatoVariety>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => 0m))
            .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => 0));

        CreateMap<UpdateTomatoVarietyDto, TomatoVariety>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // TomatoPhoto mappings
        CreateMap<TomatoPhoto, TomatoPhotoDto>()
            .ForMember(dest => dest.UploaderName, opt => opt.MapFrom(src => src.User.DisplayName ?? src.User.UserName));

        // Tag mappings
        CreateMap<Tag, TagDto>();

        // Vote mappings
        CreateMap<Vote, VoteDto>();
        CreateMap<CreateVoteDto, Vote>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Comment mappings
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.DisplayName ?? src.User.UserName))
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies.Where(r => r.IsApproved)));

        CreateMap<CreateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false));

        // User activity mappings
        CreateMap<UserActivity, UserActivityDto>()
            .ForMember(dest => dest.TomatoVarietyName, opt => opt.MapFrom(src => src.TomatoVariety!.CommonName));

        // Version mappings
        CreateMap<TomatoVarietyVersion, TomatoVarietyVersionDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.DisplayName ?? src.User.UserName))
            .ForMember(dest => dest.ReviewedByUserName, opt => opt.MapFrom(src => src.ReviewedByUser!.DisplayName ?? src.ReviewedByUser.UserName));

        // User mappings
        CreateMap<ApplicationUser, UserProfileDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.JoinedAt))
            .ForMember(dest => dest.LastActiveAt, opt => opt.MapFrom(src => src.LastLoginAt))
            .ForMember(dest => dest.ContributionsCount, opt => opt.Ignore()) // Will be calculated
            .ForMember(dest => dest.PhotosCount, opt => opt.Ignore()) // Will be calculated
            .ForMember(dest => dest.RecentActivities, opt => opt.Ignore()); // Will be populated separately
    }
}
