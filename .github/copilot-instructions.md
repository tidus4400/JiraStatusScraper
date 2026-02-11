# GitHub Copilot Context: Jira Status Scraper

## Project Overview
A .NET 10 console application that fetches issue changelog data from Jira Cloud REST API (`https://prugramin.atlassian.net`), specifically tracking status transitions with automatic pagination.

**Status:** ✅ Fully implemented, builds successfully, ready for production use.

---

## Quick Facts

- **Framework:** .NET 10
- **Language:** C# 12
- **API:** Jira Cloud REST API v3
- **Authentication:** Basic Auth (email + API token)
- **Main Feature:** Automatic pagination for changelog retrieval
- **Jira Instance:** https://prugramin.atlassian.net

---

## Project Structure

```
JiraStatusScraper/
├── src/JiraStatusScraper/
│   ├── Program.cs (105 lines)           # Main entry point with DI
│   ├── Configuration/
│   │   └── JiraSettings.cs              # Config model
│   ├── Models/
│   │   ├── JiraUser.cs                  # User record
│   │   ├── JiraIssue.cs                 # Issue/Status models
│   │   ├── Changelog.cs                 # API response models
│   │   └── StatusChange.cs              # Output model
│   ├── Services/
│   │   └── JiraClient.cs (116 lines)    # HTTP client with pagination
│   ├── appsettings.json                 # Default config
│   └── appsettings.local.json.example   # Credentials template
├── Documentation/
│   ├── README.md
│   ├── QUICKSTART.md
│   ├── IMPLEMENTATION.md
│   ├── DEPLOYMENT_CHECKLIST.md
│   ├── PROJECT_SUMMARY.md
│   └── PROJECT_STRUCTURE.md
└── setup.sh                              # Interactive setup script
```

---

## Key Implementation Details

### Important Fix Applied
**Issue:** `ToString` property name conflict with `object.ToString()` method  
**Solution:** Renamed to `ToStringValue` with `[JsonPropertyName("toString")]` attribute in `ChangelogItem` record

### Models
- All models use C# **records** for immutability
- Nullable reference types enabled
- JSON deserialization via `System.Text.Json`

### JiraClient Methods
1. `GetIssueAsync(string issueKey)` - Fetch basic issue info
2. `GetStatusChangesAsync(string issueKey)` - **Main method** - Fetch all status changes with auto-pagination
3. `GetFullChangelogAsync(string issueKey)` - Fetch complete changelog

### Pagination Logic
- Fetches changelog in chunks of 100 entries
- Loops until `IsLast == true`
- Automatically handles issues with 100+ status changes

### Configuration
- Uses Microsoft.Extensions.Configuration
- Layered config: `appsettings.json` → `appsettings.local.json`
- `appsettings.local.json` is **git-ignored** for security

---

## Common Commands

```bash
# Navigate to project
cd /Users/tidus4400/Projects/JiraStatusScraper/src/JiraStatusScraper

# Build
dotnet build

# Run (interactive)
dotnet run

# Run (with issue key)
dotnet run PROJ-123

# Clean build
dotnet clean && dotnet build

# Setup credentials (first time)
cp appsettings.local.json.example appsettings.local.json
# Then edit with email and API token
```

---

## NuGet Packages

- `Microsoft.Extensions.Http` (10.0.3) - Typed HttpClient
- `Microsoft.Extensions.Configuration.Json` (10.0.3) - JSON config

---

## API Endpoints Used

### Get Issue
```
GET /rest/api/3/issue/{issueKey}?fields=summary,status
```

### Get Changelog (paginated)
```
GET /rest/api/3/issue/{issueKey}/changelog?startAt={startAt}&maxResults={maxResults}
```

**Response fields used:**
- `startAt`, `maxResults`, `total`, `isLast` - Pagination
- `values[]` - Array of changelog history entries
  - `id`, `author`, `created`, `items[]`
  - `items[].field`, `.from`, `.fromString`, `.to`, `.toString`

---

## Code Style & Conventions

- **Naming:** PascalCase for public members, camelCase for private
- **Async:** All I/O operations are async with CancellationToken support
- **Error Handling:** Use `EnsureSuccessStatusCode()`, graceful error messages
- **Documentation:** XML comments on public methods
- **Modern C#:** Records, top-level statements, nullable types, LINQ

---

## Security Notes

⚠️ **Never commit these files:**
- `appsettings.local.json` - Contains API token
- `.env` files
- Any file with credentials

✅ **Safe to commit:**
- `appsettings.json` - Default/placeholder values only
- `appsettings.local.json.example` - Template with no real credentials

---

## Known Issues & Limitations

### Current Limitations
- Only fetches one issue at a time (no bulk/JQL support)
- Console output only (no CSV/JSON export)
- Jira Cloud only (not Server/Data Center)
- No retry logic for transient failures
- No caching

### Recent Fixes
- ✅ Fixed `ToString` property conflict in `ChangelogItem` model (Feb 11, 2026)

---

## Future Enhancement Ideas

### Phase 2 - Export
- [ ] CSV export functionality
- [ ] JSON export functionality
- [ ] File output options

### Phase 3 - Bulk Operations
- [ ] JQL query support for multiple issues
- [ ] Batch processing
- [ ] Progress indicators

### Phase 4 - Advanced Features
- [ ] Date range filtering
- [ ] Specific status transition filters
- [ ] Author filtering
- [ ] Retry policies with Polly
- [ ] Caching layer

### Phase 5 - Quality
- [ ] Unit tests
- [ ] Integration tests
- [ ] Docker containerization
- [ ] CI/CD pipeline

---

## Troubleshooting

### Build Errors
```bash
# If build fails, try:
dotnet clean
dotnet restore
dotnet build
```

### "ToString" Compilation Error
**Already fixed!** The `ChangelogItem.ToString` property was renamed to `ToStringValue`.

### HTTP 401 (Unauthorized)
- Check email is correct in `appsettings.local.json`
- Verify API token is valid
- Get new token from: https://id.atlassian.com/manage-profile/security/api-tokens

### HTTP 404 (Not Found)
- Verify issue key exists
- Check you have permission to view the issue

### No Status Changes Found
- Normal for newly created issues that haven't changed status
- Issue may have been created directly in final status

---

## Development Workflow

### Making Changes

1. **Edit source files** in `src/JiraStatusScraper/`
2. **Build:** `dotnet build`
3. **Test:** `dotnet run ISSUE-KEY`
4. **Validate:** Check output matches expectations

### Adding New Features

1. Add models to `Models/` if needed
2. Add methods to `JiraClient.cs` for new API calls
3. Update `Program.cs` to use new functionality
4. Update documentation in `README.md`
5. Test with real Jira instance

### Git Workflow

```bash
# Check status
git status

# Stage changes
git add .

# Commit with descriptive message
git commit -m "feat: add export to CSV functionality"

# Push to main
git push origin main
```

---

## Testing Checklist

- [ ] Issue with no status changes
- [ ] Issue with 1-5 status changes
- [ ] Issue with 100+ status changes (pagination test)
- [ ] Invalid issue key (404 test)
- [ ] Invalid credentials (401 test)
- [ ] Issue without permission (403 test)
- [ ] Interactive mode (no args)
- [ ] Command-line mode (with args)

---

## Quick Reference: Key Files

### Must Understand
- `Program.cs` - Application entry, DI setup, config loading
- `Services/JiraClient.cs` - Core API client logic, pagination
- `Models/Changelog.cs` - API response models (note: ToStringValue property)

### Configuration
- `appsettings.json` - Default settings (checked in)
- `appsettings.local.json` - User credentials (git-ignored, create from .example)

### Documentation
- `README.md` - User-facing documentation
- `IMPLEMENTATION.md` - Technical details
- `QUICKSTART.md` - 3-minute setup guide

---

## Dependencies Graph

```
Program.cs
├── Microsoft.Extensions.Configuration (JSON config)
├── Microsoft.Extensions.DependencyInjection (DI container)
└── JiraClient
    ├── HttpClient (Microsoft.Extensions.Http)
    └── Models
        ├── JiraUser
        ├── JiraIssue
        ├── Changelog (ChangelogResponse, ChangelogHistory, ChangelogItem)
        └── StatusChange
```

---

## Typical User Flow

1. User runs: `dotnet run PROJ-123`
2. App loads config from appsettings.json + appsettings.local.json
3. Validates credentials present
4. Creates HttpClient with auth headers
5. Calls `JiraClient.GetIssueAsync()` - displays issue summary
6. Calls `JiraClient.GetStatusChangesAsync()` - automatic pagination loop
7. Filters changelog for `field == "status"`
8. Displays chronologically ordered status changes
9. Shows count and exits

---

## Example Output

```
Fetching issue PROJ-123...

Issue: PROJ-123 - Implement user authentication
Current Status: Done

Status Change History:
--------------------------------------------------------------------------------
[2024-01-15 09:30:00] PROJ-123: (none) → To Do (by John Doe)
[2024-01-16 14:22:00] PROJ-123: To Do → In Progress (by Jane Smith)
[2024-01-18 11:45:00] PROJ-123: In Progress → Code Review (by Jane Smith)
[2024-01-19 10:15:00] PROJ-123: Code Review → Done (by John Doe)
--------------------------------------------------------------------------------
Total status changes: 4
```

---

## When Resuming Work

### First Time After Clone
```bash
cd /Users/tidus4400/Projects/JiraStatusScraper
./setup.sh
# Or manually:
cd src/JiraStatusScraper
cp appsettings.local.json.example appsettings.local.json
# Edit appsettings.local.json with credentials
dotnet restore
dotnet build
```

### Regular Development
```bash
cd /Users/tidus4400/Projects/JiraStatusScraper/src/JiraStatusScraper
dotnet build
dotnet run PROJ-123
```

### Key Questions to Ask When Starting
1. What's the goal? (feature, bug fix, documentation)
2. Which files need changes?
3. Do we need to add new NuGet packages?
4. Do we need new models for API responses?
5. How do we test the changes?

---

## Copilot Best Practices for This Project

### Code Generation
- Use **records** for DTOs/models
- Enable **nullable reference types**
- Use **async/await** for all I/O
- Add **XML documentation** to public APIs
- Follow existing **naming conventions**

### When Adding Features
1. Consider if new models needed in `Models/`
2. Add methods to `JiraClient.cs` (keep it focused on API)
3. Update `Program.cs` for CLI integration
4. Update documentation
5. Test with real Jira instance

### When Fixing Bugs
1. Reproduce the issue first
2. Add error handling if missing
3. Update documentation if behavior changes
4. Test edge cases

---

## Contact & Resources

- **Jira Instance:** https://prugramin.atlassian.net
- **Jira API Docs:** https://developer.atlassian.com/cloud/jira/platform/rest/v3/
- **API Token Management:** https://id.atlassian.com/manage-profile/security/api-tokens
- **.NET Downloads:** https://dotnet.microsoft.com/download

---

**Last Updated:** February 11, 2026  
**Status:** ✅ Fully implemented and working  
**Build Status:** ✅ Compiles successfully  
**Ready for:** Production use, feature enhancements, or maintenance

