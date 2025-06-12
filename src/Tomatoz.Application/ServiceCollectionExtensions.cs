using Microsoft.Extensions.DependencyInjection;
using Tomatoz.Application.Interfaces;
using Tomatoz.Application.Services;

namespace Tomatoz.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<ITomatoVarietyService, TomatoVarietyService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITomatoPhotoService, TomatoPhotoService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IModerationService, ModerationService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ISearchService, SearchService>();

        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions));

        // Register MediatR if needed
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}
