using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tomatoz.Application.Interfaces;
using Tomatoz.Infrastructure.Data;
using Tomatoz.Infrastructure.Repositories;

namespace Tomatoz.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Tomatoz.Infrastructure")));

        // Register repositories
        services.AddScoped<ITomatoVarietyRepository, TomatoVarietyRepository>();
        services.AddScoped<ITomatoVarietyVersionRepository, TomatoVarietyVersionRepository>();
        services.AddScoped<ITomatoPhotoRepository, TomatoPhotoRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<IUserFollowsVarietyRepository, UserFollowsVarietyRepository>();

        return services;
    }
}
