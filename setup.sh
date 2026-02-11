#!/bin/bash
# Setup script for Jira Status Scraper
# This script helps you get started quickly

set -e

echo "🚀 Jira Status Scraper - Setup Script"
echo "======================================"
echo ""

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ Error: .NET SDK not found"
    echo "Please install .NET 10 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"
echo ""

# Navigate to project directory
cd "$(dirname "$0")/src/JiraStatusScraper"

# Check if appsettings.local.json exists
if [ -f "appsettings.local.json" ]; then
    echo "⚠️  appsettings.local.json already exists"
    read -p "Do you want to overwrite it? (y/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        echo "Skipping configuration file creation"
    else
        cp appsettings.local.json.example appsettings.local.json
        echo "✅ Created appsettings.local.json"
    fi
else
    cp appsettings.local.json.example appsettings.local.json
    echo "✅ Created appsettings.local.json"
fi

echo ""
echo "📝 Next steps:"
echo "1. Get your Jira API token from:"
echo "   https://id.atlassian.com/manage-profile/security/api-tokens"
echo ""
echo "2. Edit appsettings.local.json with your credentials:"
echo "   - Email: your-email@company.com"
echo "   - ApiToken: your-api-token-here"
echo ""
echo "3. Run the application:"
echo "   dotnet run PROJ-123"
echo ""

# Ask if user wants to restore packages now
read -p "Restore NuGet packages now? (Y/n): " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]] || [[ -z $REPLY ]]; then
    echo "📦 Restoring packages..."
    dotnet restore
    echo "✅ Packages restored"
    echo ""
    
    echo "🔨 Building project..."
    dotnet build --no-restore
    echo "✅ Build complete"
else
    echo "⏭️  Skipped package restore (run 'dotnet restore' manually)"
fi

echo ""
echo "🎉 Setup complete!"
echo "Edit appsettings.local.json with your credentials, then run:"
echo "  cd src/JiraStatusScraper"
echo "  dotnet run PROJ-123"
echo ""

