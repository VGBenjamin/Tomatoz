#!/bin/bash
# External Authentication Setup Script
# This script helps you set up external authentication providers for development

echo "🔐 External Authentication Setup for Tomatoz"
echo "============================================="
echo ""

cd "$(dirname "$0")/src/Tomatoz/Tomatoz"

echo "📍 Current directory: $(pwd)"
echo ""

# Check if we're in the right directory
if [ ! -f "Tomatoz.csproj" ]; then
    echo "❌ Error: This script must be run from the root of the Tomatoz repository"
    echo "   Expected to find Tomatoz.csproj in src/Tomatoz/Tomatoz/"
    exit 1
fi

echo "🔧 Setting up user secrets..."
echo ""

# Google Authentication
echo "🔍 Setting up Google Authentication"
echo "Please enter your Google OAuth Client ID (or press Enter to skip):"
read -r GOOGLE_CLIENT_ID

if [ -n "$GOOGLE_CLIENT_ID" ]; then
    echo "Please enter your Google OAuth Client Secret:"
    read -r GOOGLE_CLIENT_SECRET
    
    dotnet user-secrets set "Authentication:Google:ClientId" "$GOOGLE_CLIENT_ID"
    dotnet user-secrets set "Authentication:Google:ClientSecret" "$GOOGLE_CLIENT_SECRET"
    echo "✅ Google authentication configured"
else
    echo "⏭️  Skipping Google authentication"
fi

echo ""

# Microsoft Authentication
echo "🔍 Setting up Microsoft Authentication"
echo "Please enter your Microsoft OAuth Client ID (or press Enter to skip):"
read -r MICROSOFT_CLIENT_ID

if [ -n "$MICROSOFT_CLIENT_ID" ]; then
    echo "Please enter your Microsoft OAuth Client Secret:"
    read -r MICROSOFT_CLIENT_SECRET
    
    dotnet user-secrets set "Authentication:Microsoft:ClientId" "$MICROSOFT_CLIENT_ID"
    dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "$MICROSOFT_CLIENT_SECRET"
    echo "✅ Microsoft authentication configured"
else
    echo "⏭️  Skipping Microsoft authentication"
fi

echo ""

# GitHub Authentication
echo "🔍 Setting up GitHub Authentication"
echo "Please enter your GitHub OAuth Client ID (or press Enter to skip):"
read -r GITHUB_CLIENT_ID

if [ -n "$GITHUB_CLIENT_ID" ]; then
    echo "Please enter your GitHub OAuth Client Secret:"
    read -r GITHUB_CLIENT_SECRET
    
    dotnet user-secrets set "Authentication:GitHub:ClientId" "$GITHUB_CLIENT_ID"
    dotnet user-secrets set "Authentication:GitHub:ClientSecret" "$GITHUB_CLIENT_SECRET"
    echo "✅ GitHub authentication configured"
else
    echo "⏭️  Skipping GitHub authentication"
fi

echo ""
echo "🎉 Setup complete!"
echo ""
echo "📋 Summary of configured user secrets:"
dotnet user-secrets list | grep "Authentication:"
echo ""
echo "🚀 You can now run the application and see the external authentication providers on the login page."
echo ""
echo "📖 For detailed setup instructions, see Doc/external-authentication-setup.md"
