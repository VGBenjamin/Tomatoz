namespace Tomatoz;

public static class AuthenticationConfiguration
{
    public static void AddExternalAuthenticationProviders(this IServiceCollection services, IConfiguration configuration)
    {
        var authBuilder = services.AddAuthentication();        // Google Authentication
        var googleClientId = configuration["Authentication:Google:ClientId"];
        var googleClientSecret = configuration["Authentication:Google:ClientSecret"];
        if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
        {
            authBuilder.AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = googleClientId;
                googleOptions.ClientSecret = googleClientSecret;
                googleOptions.SaveTokens = true;
                // Explicitly set the callback path to ensure it matches Google's expectations
                googleOptions.CallbackPath = "/signin-google";
            });
        }

        // Microsoft Account Authentication
        var microsoftClientId = configuration["Authentication:Microsoft:ClientId"];
        var microsoftClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
        if (!string.IsNullOrEmpty(microsoftClientId) && !string.IsNullOrEmpty(microsoftClientSecret))
        {
            authBuilder.AddMicrosoftAccount(microsoftOptions =>
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
            authBuilder.AddGitHub(githubOptions =>
            {
                githubOptions.ClientId = githubClientId;
                githubOptions.ClientSecret = githubClientSecret;
                githubOptions.SaveTokens = true;
                githubOptions.Scope.Add("user:email");
            });
        }
    }
}
