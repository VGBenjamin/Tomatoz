#!/bin/bash

# External Authentication Activation Script for Tomatoz
# This script enables external authentication in Program.cs

echo "🔐 Tomatoz External Authentication Activator"
echo "============================================="

PROGRAM_FILE="src/Tomatoz/Tomatoz/Program.cs"

if [ ! -f "$PROGRAM_FILE" ]; then
    echo "❌ Error: Program.cs not found at $PROGRAM_FILE"
    echo "Please run this script from the Tomatoz root directory."
    exit 1
fi

echo "📝 Activating external authentication in Program.cs..."

# Replace the comment block with the actual method call
sed -i 's|// Add external authentication providers conditionally  \
// TODO: External authentication will be configured when needed\
// Configuration is ready in user secrets|// Add external authentication providers conditionally  \
builder.Services.AddExternalAuthenticationProviders(builder.Configuration);|' "$PROGRAM_FILE"

if [ $? -eq 0 ]; then
    echo "✅ External authentication activated successfully!"
    echo ""
    echo "📋 Next steps:"
    echo "1. Configure credentials for your chosen providers (see Doc/external-authentication-setup.md)"
    echo "2. Run 'dotnet build' to verify compilation"
    echo "3. Start the application and check the login page"
    echo ""
    echo "🔧 To configure credentials, use:"
    echo "   dotnet user-secrets set \"Authentication:Google:ClientId\" \"your-client-id\""
    echo "   dotnet user-secrets set \"Authentication:Google:ClientSecret\" \"your-client-secret\""
else
    echo "❌ Failed to activate external authentication"
    echo "Please manually edit Program.cs according to the documentation."
fi
