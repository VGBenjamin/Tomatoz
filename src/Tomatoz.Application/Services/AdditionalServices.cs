using AutoMapper;
using Tomatoz.Application.Interfaces;
using Tomatoz.Shared.DTOs;
using Tomatoz.Shared.Enums;
using Tomatoz.Shared.Models;

namespace Tomatoz.Application.Services;

public class TomatoPhotoService : ITomatoPhotoService
{
    private readonly ITomatoPhotoRepository _photoRepository;
    private readonly ITomatoVarietyRepository _varietyRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TomatoPhotoService(
        ITomatoPhotoRepository photoRepository,
        ITomatoVarietyRepository varietyRepository,
        IUserService userService,
        IMapper mapper)
    {
        _photoRepository = photoRepository;
        _varietyRepository = varietyRepository;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<List<TomatoPhotoDto>> GetByVarietyIdAsync(int varietyId)
    {
        var photos = await _photoRepository.GetApprovedPhotosByVarietyAsync(varietyId);
        return _mapper.Map<List<TomatoPhotoDto>>(photos);
    }

    public async Task<TomatoPhotoDto> UploadAsync(UploadPhotoDto uploadDto, Stream fileStream, string fileName, string userId)
    {
        // Validate variety exists
        var variety = await _varietyRepository.GetByIdAsync(uploadDto.TomatoVarietyId);
        if (variety == null)
            throw new ArgumentException("Tomato variety not found");

        // Generate unique filename
        var fileExtension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine("uploads", "photos", uniqueFileName);

        // Save file (in real implementation, this would save to cloud storage)
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        
        using var fileStreamOut = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamOut);

        var photo = new TomatoPhoto
        {
            TomatoVarietyId = uploadDto.TomatoVarietyId,
            UserId = userId,
            FileName = uniqueFileName,
            FilePath = filePath,
            Title = uploadDto.Title,
            Description = uploadDto.Description,
            PhotoType = uploadDto.PhotoType,
            DisplayOrder = uploadDto.DisplayOrder,
            UploadedAt = DateTime.UtcNow,
            IsApproved = false, // Requires moderation
            FileSizeBytes = fileStream.Length
        };

        var savedPhoto = await _photoRepository.AddAsync(photo);
        
        // Add user activity
        await _userService.AddUserActivityAsync(userId, ActivityType.AddPhoto, 5, uploadDto.TomatoVarietyId, $"Uploaded photo: {uploadDto.Title}");

        return _mapper.Map<TomatoPhotoDto>(savedPhoto);
    }

    public async Task<bool> DeleteAsync(int photoId, string userId)
    {
        var photo = await _photoRepository.GetByIdAsync(photoId);
        if (photo == null || photo.UserId != userId)
            return false;

        // Delete file
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo.FilePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return await _photoRepository.DeleteAsync(photoId);
    }

    public async Task<bool> ApproveAsync(int photoId, string moderatorUserId)
    {
        var photo = await _photoRepository.GetByIdAsync(photoId);
        if (photo == null)
            return false;

        photo.IsApproved = true;
        await _photoRepository.UpdateAsync(photo);

        // Add points to uploader
        await _userService.AddUserActivityAsync(photo.UserId, ActivityType.Approve, 3, photo.TomatoVarietyId, "Photo approved");

        return true;
    }

    public async Task<bool> UpdateOrderAsync(int photoId, int newOrder, string userId)
    {
        var photo = await _photoRepository.GetByIdAsync(photoId);
        if (photo == null || photo.UserId != userId)
            return false;

        photo.DisplayOrder = newOrder;
        await _photoRepository.UpdateAsync(photo);
        return true;
    }
}

public class VoteService : IVoteService
{
    private readonly IVoteRepository _voteRepository;
    private readonly ITomatoVarietyRepository _varietyRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public VoteService(
        IVoteRepository voteRepository,
        ITomatoVarietyRepository varietyRepository,
        IUserService userService,
        IMapper mapper)
    {
        _voteRepository = voteRepository;
        _varietyRepository = varietyRepository;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<VoteDto?> GetUserVoteAsync(int varietyId, string userId, VoteType voteType)
    {
        var vote = await _voteRepository.GetUserVoteAsync(varietyId, userId, voteType);
        return vote != null ? _mapper.Map<VoteDto>(vote) : null;
    }

    public async Task<VoteDto> CreateOrUpdateVoteAsync(CreateVoteDto voteDto, string userId)
    {
        var existingVote = await _voteRepository.GetUserVoteAsync(voteDto.TomatoVarietyId, userId, voteDto.VoteType);

        if (existingVote != null)
        {
            existingVote.Rating = voteDto.Rating;
            existingVote.UpdatedAt = DateTime.UtcNow;
            await _voteRepository.UpdateAsync(existingVote);
            return _mapper.Map<VoteDto>(existingVote);
        }
        else
        {
            var newVote = new Vote
            {
                TomatoVarietyId = voteDto.TomatoVarietyId,
                UserId = userId,
                VoteType = voteDto.VoteType,
                Rating = voteDto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            var savedVote = await _voteRepository.AddAsync(newVote);
            
            // Add user activity
            await _userService.AddUserActivityAsync(userId, ActivityType.Vote, 1, voteDto.TomatoVarietyId, $"Voted on {voteDto.VoteType}");

            // Update variety average rating
            await UpdateVarietyRating(voteDto.TomatoVarietyId);

            return _mapper.Map<VoteDto>(savedVote);
        }
    }

    public async Task<bool> DeleteVoteAsync(int varietyId, string userId, VoteType voteType)
    {
        var vote = await _voteRepository.GetUserVoteAsync(varietyId, userId, voteType);
        if (vote == null)
            return false;

        await _voteRepository.DeleteAsync(vote.Id);
        await UpdateVarietyRating(varietyId);
        return true;
    }

    public async Task<Dictionary<VoteType, decimal>> GetAverageRatingsAsync(int varietyId)
    {
        return await _voteRepository.GetAverageRatingsAsync(varietyId);
    }

    private async Task UpdateVarietyRating(int varietyId)
    {
        var averageRatings = await _voteRepository.GetAverageRatingsAsync(varietyId);
        var overallRating = averageRatings.Values.Any() ? averageRatings.Values.Average() : 0;

        var variety = await _varietyRepository.GetByIdAsync(varietyId);
        if (variety != null)
        {
            variety.AverageRating = Math.Round(overallRating, 1);
            variety.UpdatedAt = DateTime.UtcNow;
            await _varietyRepository.UpdateAsync(variety);
        }
    }
}

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITomatoVarietyRepository _varietyRepository;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public CommentService(
        ICommentRepository commentRepository,
        ITomatoVarietyRepository varietyRepository,
        IUserService userService,
        INotificationService notificationService,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _varietyRepository = varietyRepository;
        _userService = userService;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> GetByVarietyIdAsync(int varietyId)
    {
        var comments = await _commentRepository.GetApprovedCommentsByVarietyAsync(varietyId);
        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto commentDto, string userId)
    {
        var variety = await _varietyRepository.GetByIdAsync(commentDto.TomatoVarietyId);
        if (variety == null)
            throw new ArgumentException("Tomato variety not found");

        var comment = new Comment
        {
            TomatoVarietyId = commentDto.TomatoVarietyId,
            UserId = userId,
            Content = commentDto.Content,
            ParentCommentId = commentDto.ParentCommentId,
            CreatedAt = DateTime.UtcNow,
            IsApproved = false // Requires moderation
        };

        var savedComment = await _commentRepository.AddAsync(comment);

        // Add user activity
        await _userService.AddUserActivityAsync(userId, ActivityType.AddComment, 2, commentDto.TomatoVarietyId, "Added comment");

        // Notify followers
        await _notificationService.NotifyNewCommentAsync(commentDto.TomatoVarietyId, userId);

        return _mapper.Map<CommentDto>(savedComment);
    }

    public async Task<bool> UpdateAsync(int commentId, string content, string userId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment == null || comment.UserId != userId)
            return false;

        comment.Content = content;
        comment.UpdatedAt = DateTime.UtcNow;
        comment.IsApproved = false; // Re-approval needed after edit
        await _commentRepository.UpdateAsync(comment);
        return true;
    }

    public async Task<bool> DeleteAsync(int commentId, string userId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment == null || comment.UserId != userId)
            return false;

        return await _commentRepository.DeleteAsync(commentId);
    }

    public async Task<bool> ApproveAsync(int commentId, string moderatorUserId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment == null)
            return false;

        comment.IsApproved = true;
        await _commentRepository.UpdateAsync(comment);

        // Add points to commenter
        await _userService.AddUserActivityAsync(comment.UserId, ActivityType.Approve, 2, comment.TomatoVarietyId, "Comment approved");

        return true;
    }
}

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TagService(ITagRepository tagRepository, IUserService userService, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _tagRepository.GetAllAsync();
        return _mapper.Map<List<TagDto>>(tags);
    }

    public async Task<List<TagDto>> SearchAsync(string query)
    {
        var tags = await _tagRepository.SearchAsync(query);
        return _mapper.Map<List<TagDto>>(tags);
    }

    public async Task<TagDto> CreateAsync(string name, string? description, string userId)
    {
        var existingTag = await _tagRepository.GetByNameAsync(name);
        if (existingTag != null)
            return _mapper.Map<TagDto>(existingTag);

        var tag = new Tag
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UsageCount = 0
        };

        var savedTag = await _tagRepository.AddAsync(tag);
        return _mapper.Map<TagDto>(savedTag);
    }

    public async Task<List<TagDto>> GetPopularTagsAsync(int count = 20)
    {
        var tags = await _tagRepository.GetPopularTagsAsync(count);
        return _mapper.Map<List<TagDto>>(tags);
    }
}
