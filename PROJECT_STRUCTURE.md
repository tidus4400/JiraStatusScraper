# Project Structure

```
JiraStatusScraper/
│
├── 📄 README.md                          # Main documentation
├── ⚡ QUICKSTART.md                      # 3-minute setup guide  
├── 🔧 IMPLEMENTATION.md                  # Technical details
├── ✅ DEPLOYMENT_CHECKLIST.md            # Validation checklist
├── 📋 PROJECT_SUMMARY.md                 # Complete overview
├── 🚀 setup.sh                           # Interactive setup script (executable)
│
├── 🔒 .gitignore                         # Git ignore rules (protects credentials)
├── 📦 JiraStatusScraper.slnx             # Solution file
│
└── src/
    └── JiraStatusScraper/
        │
        ├── 📄 Program.cs                 # ⭐ Main entry point (105 lines)
        ├── 📦 JiraStatusScraper.csproj   # Project file (.NET 10)
        │
        ├── ⚙️ Configuration/
        │   └── JiraSettings.cs           # Configuration model
        │
        ├── 📊 Models/
        │   ├── JiraUser.cs               # User record
        │   ├── JiraIssue.cs              # Issue, fields, status
        │   ├── Changelog.cs              # Changelog response models
        │   └── StatusChange.cs           # Status change output model
        │
        ├── 🌐 Services/
        │   └── JiraClient.cs             # ⭐ HTTP client with pagination (116 lines)
        │
        └── 🔐 Config Files/
            ├── appsettings.json          # Default config (BaseUrl preset)
            ├── appsettings.local.json.example  # Credentials template
            └── appsettings.local.json    # ❗ YOU CREATE THIS (git-ignored)
```

## File Count Summary

| Category | Count | Details |
|----------|-------|---------|
| **Source Files** | 7 | C# implementation files |
| **Configuration** | 3 | Settings and project files |
| **Documentation** | 5 | README, guides, checklists |
| **Scripts** | 1 | Setup automation |
| **Solution** | 2 | .slnx and .gitignore |
| **TOTAL** | **18** | **Complete project** |

## Lines of Code

| File | Lines | Purpose |
|------|-------|---------|
| `Program.cs` | 105 | Main application logic |
| `JiraClient.cs` | 116 | API client with pagination |
| `Changelog.cs` | 31 | Changelog models |
| `JiraIssue.cs` | 18 | Issue models |
| `StatusChange.cs` | 13 | Output model |
| `JiraSettings.cs` | 8 | Config model |
| `JiraUser.cs` | 7 | User model |
| **TOTAL** | **~300** | **Clean, focused code** |

## Key Directories

### `/src/JiraStatusScraper/`
The main application directory containing all source code.

### `/src/JiraStatusScraper/Models/`
Data models for Jira API responses and output formatting.

### `/src/JiraStatusScraper/Services/`
Business logic - the JiraClient HTTP service.

### `/src/JiraStatusScraper/Configuration/`
Configuration classes and settings management.

## Important Files to Know

### 🔴 MUST EDIT
- `appsettings.local.json` - **You must create this with your credentials**

### 🟡 MAY EDIT
- `appsettings.json` - Only if you need to change BaseUrl

### 🟢 READ ONLY
- All `.cs` files - The implementation (don't modify unless extending)
- `appsettings.local.json.example` - Template file
- Documentation files - Reference materials

## Configuration Files Explained

```
appsettings.json                    # Checked into git
├── BaseUrl: preset               # https://prugramin.atlassian.net
├── Email: placeholder            # your-email@example.com
└── ApiToken: placeholder         # your-api-token-here

appsettings.local.json.example      # Checked into git (template)
├── Same structure as above       # Safe to share
└── Use this as a template        # Copy to create local config

appsettings.local.json              # ❗ NOT in git (you create)
├── Your actual credentials       # KEEP SECRET
├── Overrides appsettings.json    # Takes precedence
└── Git-ignored for security      # Never commit this
```

## Execution Flow

```
1. dotnet run PROJ-123
   ↓
2. Program.cs loads configuration
   ↓
3. Validates credentials present
   ↓
4. Creates HttpClient with auth
   ↓
5. Injects into JiraClient
   ↓
6. Calls GetIssueAsync()
   ↓
7. Displays issue details
   ↓
8. Calls GetStatusChangesAsync()
   ↓
9. Automatic pagination loop
   ↓
10. Filters for status changes
    ↓
11. Displays chronologically
    ↓
12. Done! ✅
```

## Dependency Graph

```
Program.cs
├── Configuration
│   ├── Microsoft.Extensions.Configuration
│   └── JiraSettings.cs
├── DI Container
│   └── Microsoft.Extensions.DependencyInjection
├── Services
│   └── JiraClient.cs
│       ├── HttpClient (from Microsoft.Extensions.Http)
│       └── Models
│           ├── JiraUser.cs
│           ├── JiraIssue.cs
│           ├── Changelog.cs
│           └── StatusChange.cs
```

## NuGet Package Tree

```
JiraStatusScraper.csproj
├── Microsoft.Extensions.Http (10.0.3)
│   ├── Microsoft.Extensions.DependencyInjection
│   ├── Microsoft.Extensions.Logging
│   └── System.Net.Http
└── Microsoft.Extensions.Configuration.Json (10.0.3)
    ├── Microsoft.Extensions.Configuration
    ├── Microsoft.Extensions.FileProviders
    └── System.Text.Json
```

## Build Output Structure

```
bin/
└── Debug/
    └── net10.0/
        ├── JiraStatusScraper.dll
        ├── JiraStatusScraper.exe (on Windows)
        ├── appsettings.json          # Copied
        ├── appsettings.local.json    # Copied (if exists)
        └── [dependencies...]
```

## Git-Ignored Items

```
.gitignore excludes:
├── bin/                    # Build output
├── obj/                    # Build intermediates
├── appsettings.local.json  # ⭐ YOUR CREDENTIALS
├── .vs/                    # Visual Studio
├── .idea/                  # JetBrains Rider
└── Various temp files
```

## Quick Commands Reference

```bash
# Navigate to project
cd /Users/tidus4400/Projects/JiraStatusScraper

# Setup (one time)
./setup.sh

# Build
cd src/JiraStatusScraper
dotnet build

# Run interactive
dotnet run

# Run with issue key
dotnet run PROJ-123

# Clean build
dotnet clean && dotnet build

# Restore packages
dotnet restore
```

## Entry Points

| File | Purpose | When to Use |
|------|---------|-------------|
| `setup.sh` | Initial setup | First time only |
| `Program.cs` | Application | Every run (`dotnet run`) |
| `README.md` | Documentation | When you need help |
| `QUICKSTART.md` | Fast setup | When in a hurry |

---

**Navigate this structure to understand the complete implementation!** 🗺️

