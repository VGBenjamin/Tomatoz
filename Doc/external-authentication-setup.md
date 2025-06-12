# External Authentication Setup Guide

This guide will help you configure external authentication providers (Google, Microsoft, and GitHub) for your Tomatoz application.

## Current Status

✅ **Authentication packages installed**: Google, Microsoft Account, and GitHub authentication packages are ready
✅ **Infrastructure configured**: Authentication configuration class is implemented
✅ **User Secrets initialized**: Secure credential storage is set up
⚠️ **Manual activation required**: External authentication needs to be enabled in Program.cs

## Quick Setup

To enable external authentication, you need to:

1. **Configure credentials** for your chosen providers (see detailed steps below)
2. **Activate authentication** in Program.cs by replacing the comment with the method call
3. **Test the login page** to see external providers appear

## Enable Authentication in Code

In `Program.cs`, replace this line:
```csharp
// Add external authentication providers conditionally  
// TODO: External authentication will be configured when needed
// Configuration is ready in user secrets
```

With:
```csharp
// Add external authentication providers conditionally  
builder.Services.AddExternalAuthenticationProviders(builder.Configuration);
```

## Provider Configuration

### 1. Google Authentication

1. Go to the [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select an existing one
3. Enable the Google+ API
4. Go to "Credentials" and create a new OAuth 2.0 Client ID
5. Configure the authorized redirect URIs:
   - For development: `https://localhost:5001/signin-google` (adjust port if different)
   - For production: `https://yourdomain.com/signin-google`

**Set the credentials using User Secrets:**
```bash
dotnet user-secrets set "Authentication:Google:ClientId" "your-google-client-id"
dotnet user-secrets set "Authentication:Google:ClientSecret" "your-google-client-secret"
```

### 2. Microsoft Account Authentication

1. Go to the [Azure Portal](https://portal.azure.com/)
2. Register a new application in Azure Active Directory
3. Add a redirect URI:
   - For development: `https://localhost:5001/signin-microsoft` (adjust port if different)
   - For production: `https://yourdomain.com/signin-microsoft`

**Set the credentials using User Secrets:**
```bash
dotnet user-secrets set "Authentication:Microsoft:ClientId" "your-microsoft-client-id"
dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "your-microsoft-client-secret"
```

### 3. GitHub Authentication

1. Go to [GitHub Developer Settings](https://github.com/settings/developers)
2. Create a new OAuth App
3. Set the Authorization callback URL:
   - For development: `https://localhost:5001/signin-github` (adjust port if different)
   - For production: `https://yourdomain.com/signin-github`

**Set the credentials using User Secrets:**
```bash
dotnet user-secrets set "Authentication:GitHub:ClientId" "your-github-client-id"
dotnet user-secrets set "Authentication:GitHub:ClientSecret" "your-github-client-secret"
```

## Production Configuration

For production environments, use one of the following secure methods instead of User Secrets:

### Option 1: Environment Variables
Set the following environment variables:
- `Authentication__Google__ClientId`
- `Authentication__Google__ClientSecret`
- `Authentication__Microsoft__ClientId`
- `Authentication__Microsoft__ClientSecret`
- `Authentication__GitHub__ClientId`
- `Authentication__GitHub__ClientSecret`

### Option 2: Azure Key Vault (Recommended for Azure deployments)
Store the secrets in Azure Key Vault and configure the application to read from it.

### Option 3: Configuration Provider
Use your preferred secure configuration provider for your hosting environment.

## Verifying Configuration

1. Start the application
2. Navigate to the login page (`/Account/Login`)
3. You should see buttons for the configured external providers in the "Use another service to log in" section
4. If no providers are configured, you'll see the message about setting up external authentication services

## Security Best Practices

1. **Never commit secrets to source control**
2. **Use HTTPS in production** - External providers require HTTPS for security
3. **Configure CORS properly** if your application has a separate frontend
4. **Review scopes** - Only request the minimum permissions needed
5. **Implement proper error handling** for authentication failures
6. **Use secure session management** - The application is already configured with secure defaults

## Troubleshooting

### Common Issues:

1. **"Invalid redirect URI"**: Ensure the redirect URI in your provider configuration matches exactly what you've registered
2. **"Client ID not found"**: Check that your credentials are properly set in User Secrets or environment variables
3. **HTTPS required**: External providers require HTTPS. Use `dotnet dev-certs https --trust` for development
4. **No providers showing**: Verify that at least one set of credentials is configured correctly

### Checking User Secrets:
```bash
dotnet user-secrets list
```

### Clearing User Secrets (if needed):
```bash
dotnet user-secrets clear
```

## Advanced Configuration

The authentication providers are configured in `Extensions/AuthenticationExtensions.cs`. You can modify this file to:

- Add additional scopes
- Configure custom claim mappings
- Add more providers
- Customize the authentication flow

## Support

If you encounter issues, check:
1. The application logs for detailed error messages
2. The browser's developer tools for network errors
3. The external provider's documentation for the latest requirements
