using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

namespace Tomatoz.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddExternalAuthenticationProviders(this IServiceCollection services, IConfiguration configuration)
    {
        // Google Authentication
        var googleClientId = configuration["Authentication:Google:ClientId"];
        var googleClientSecret = configuration["Authentication:Google:ClientSecret"];
        
        if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
        {
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = googleClientId;
                googleOptions.ClientSecret = googleClientSecret;
                googleOptions.SaveTokens = true;
            });
        }

        // Microsoft Account Authentication
        var microsoftClientId = configuration["Authentication:Microsoft:ClientId"];
        var microsoftClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
        
        if (!string.IsNullOrEmpty(microsoftClientId) && !string.IsNullOrEmpty(microsoftClientSecret))
        {
            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = microsoftClientId;
                microsoftOptions.ClientSecret = microsoftClientSecret;
                microsoftOptions.SaveTokens = true;
            });
        }

        // GitHub Authentication
        var githubClientId = configuration["Authentication:GitHub:ClientId"];
        var githubClientSecret = configuration["Authentication:GitHub:ClientSecret"];
        
        if (!string.IsNullOrEmpty(githubClientId) && !string.IsNullOrEmpty(githubClientSecret))
        {
            services.AddAuthentication().AddGitHub(githubOptions =>
            {
                githubOptions.ClientId = githubClientId;
                githubOptions.ClientSecret = githubClientSecret;
                githubOptions.SaveTokens = true;
                githubOptions.Scope.Add("user:email");
            });
        }

        return services;
    }
}
