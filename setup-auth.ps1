# External Authentication Setup Script for Windows
# This script helps you set up external authentication providers for development

Write-Host "🔐 External Authentication Setup for Tomatoz" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Green
Write-Host ""

# Navigate to the correct directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectPath = Join-Path $scriptPath "src\Tomatoz\Tomatoz"

if (Test-Path $projectPath) {
    Set-Location $projectPath
    Write-Host "📍 Current directory: $(Get-Location)" -ForegroundColor Yellow
} else {
    Write-Host "❌ Error: Could not find project directory at $projectPath" -ForegroundColor Red
    Write-Host "   Please ensure you're running this script from the root of the Tomatoz repository" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Check if we're in the right directory
if (-not (Test-Path "Tomatoz.csproj")) {
    Write-Host "❌ Error: This script must be run from the root of the Tomatoz repository" -ForegroundColor Red
    Write-Host "   Expected to find Tomatoz.csproj in src/Tomatoz/Tomatoz/" -ForegroundColor Red
    exit 1
}

Write-Host "🔧 Setting up user secrets..." -ForegroundColor Blue
Write-Host ""

# Google Authentication
Write-Host "🔍 Setting up Google Authentication" -ForegroundColor Cyan
$googleClientId = Read-Host "Please enter your Google OAuth Client ID (or press Enter to skip)"

if ($googleClientId) {
    $googleClientSecret = Read-Host "Please enter your Google OAuth Client Secret" -AsSecureString
    $googleClientSecretPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($googleClientSecret))
    
    dotnet user-secrets set "Authentication:Google:ClientId" $googleClientId
    dotnet user-secrets set "Authentication:Google:ClientSecret" $googleClientSecretPlain
    Write-Host "✅ Google authentication configured" -ForegroundColor Green
} else {
    Write-Host "⏭️  Skipping Google authentication" -ForegroundColor Yellow
}

Write-Host ""

# Microsoft Authentication
Write-Host "🔍 Setting up Microsoft Authentication" -ForegroundColor Cyan
$microsoftClientId = Read-Host "Please enter your Microsoft OAuth Client ID (or press Enter to skip)"

if ($microsoftClientId) {
    $microsoftClientSecret = Read-Host "Please enter your Microsoft OAuth Client Secret" -AsSecureString
    $microsoftClientSecretPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($microsoftClientSecret))
    
    dotnet user-secrets set "Authentication:Microsoft:ClientId" $microsoftClientId
    dotnet user-secrets set "Authentication:Microsoft:ClientSecret" $microsoftClientSecretPlain
    Write-Host "✅ Microsoft authentication configured" -ForegroundColor Green
} else {
    Write-Host "⏭️  Skipping Microsoft authentication" -ForegroundColor Yellow
}

Write-Host ""

# GitHub Authentication
Write-Host "🔍 Setting up GitHub Authentication" -ForegroundColor Cyan
$githubClientId = Read-Host "Please enter your GitHub OAuth Client ID (or press Enter to skip)"

if ($githubClientId) {
    $githubClientSecret = Read-Host "Please enter your GitHub OAuth Client Secret" -AsSecureString
    $githubClientSecretPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($githubClientSecret))
    
    dotnet user-secrets set "Authentication:GitHub:ClientId" $githubClientId
    dotnet user-secrets set "Authentication:GitHub:ClientSecret" $githubClientSecretPlain
    Write-Host "✅ GitHub authentication configured" -ForegroundColor Green
} else {
    Write-Host "⏭️  Skipping GitHub authentication" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🎉 Setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Summary of configured user secrets:" -ForegroundColor Blue
$secrets = dotnet user-secrets list | Where-Object { $_ -match "Authentication:" }
if ($secrets) {
    $secrets | ForEach-Object { Write-Host "   $_" -ForegroundColor White }
} else {
    Write-Host "   No authentication secrets configured" -ForegroundColor Yellow
}
Write-Host ""
Write-Host "🚀 You can now run the application and see the external authentication providers on the login page." -ForegroundColor Green
Write-Host ""
Write-Host "📖 For detailed setup instructions, see Doc/external-authentication-setup.md" -ForegroundColor Blue
