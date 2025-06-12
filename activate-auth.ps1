# External Authentication Activation Script for Tomatoz
# This script enables external authentication in Program.cs

Write-Host "🔐 Tomatoz External Authentication Activator" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

$ProgramFile = "src\Tomatoz\Tomatoz\Program.cs"

if (-not (Test-Path $ProgramFile)) {
    Write-Host "❌ Error: Program.cs not found at $ProgramFile" -ForegroundColor Red
    Write-Host "Please run this script from the Tomatoz root directory." -ForegroundColor Yellow
    exit 1
}

Write-Host "📝 Activating external authentication in Program.cs..." -ForegroundColor Yellow

# Read the current content
$content = Get-Content $ProgramFile -Raw

# Replace the comment block with the actual method call
$newContent = $content -replace '// Add external authentication providers conditionally  \r?\n// TODO: External authentication will be configured when needed\r?\n// Configuration is ready in user secrets', '// Add external authentication providers conditionally  
builder.Services.AddExternalAuthenticationProviders(builder.Configuration);'

# Write back to file
Set-Content -Path $ProgramFile -Value $newContent

Write-Host "✅ External authentication activated successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Next steps:" -ForegroundColor Cyan
Write-Host "1. Configure credentials for your chosen providers (see Doc/external-authentication-setup.md)" -ForegroundColor White
Write-Host "2. Run 'dotnet build' to verify compilation" -ForegroundColor White
Write-Host "3. Start the application and check the login page" -ForegroundColor White
Write-Host ""
Write-Host "🔧 To configure credentials, use:" -ForegroundColor Cyan
Write-Host "   dotnet user-secrets set `"Authentication:Google:ClientId`" `"your-client-id`"" -ForegroundColor Gray
Write-Host "   dotnet user-secrets set `"Authentication:Google:ClientSecret`" `"your-client-secret`"" -ForegroundColor Gray
