# Deployment Checklist ✅

Use this checklist to verify your Jira Status Scraper is ready to use.

## Prerequisites ✓

- [ ] .NET 10 SDK installed (`dotnet --version` shows 10.x.x)
- [ ] Git repository cloned/accessible
- [ ] Terminal/command prompt access

## Configuration ⚙️

- [ ] Created `appsettings.local.json` from `appsettings.local.json.example`
- [ ] Obtained Jira API token from https://id.atlassian.com/manage-profile/security/api-tokens
- [ ] Updated `appsettings.local.json` with:
  - [ ] Your email address
  - [ ] Your API token
  - [ ] Verified BaseUrl is `https://prugramin.atlassian.net`

## First Run 🚀

### Step 1: Restore packages
```bash
cd src/JiraStatusScraper
dotnet restore
```
Expected: Packages restored successfully

### Step 2: Build project
```bash
dotnet build
```
Expected: Build succeeded with 0 errors

### Step 3: Test run (interactive mode)
```bash
dotnet run
```
- [ ] Prompted for issue key
- [ ] Enter a valid issue key (e.g., `PROJ-123`)
- [ ] Verify output shows issue details
- [ ] Verify status change history appears

### Step 4: Test run (command-line mode)
```bash
dotnet run PROJ-123
```
- [ ] Output appears immediately without prompt
- [ ] Issue details displayed
- [ ] Status changes listed

## Validation ✅

### Configuration Validation
- [ ] Running without `appsettings.local.json` shows clear error message
- [ ] Running with empty credentials shows validation error

### Error Handling
Test with invalid inputs:
- [ ] Invalid issue key → Shows 404 error message
- [ ] Wrong API token → Shows 401 authentication error
- [ ] Issue you don't have access to → Shows appropriate error

### Pagination Test
- [ ] Find an issue with many status changes (ideally 100+)
- [ ] Verify all status changes are displayed
- [ ] Check that count matches what you see in Jira UI

## Expected Output Format 📋

```
Fetching issue PROJ-123...

Issue: PROJ-123 - [Issue Summary]
Current Status: [Current Status Name]

Status Change History:
--------------------------------------------------------------------------------
[YYYY-MM-DD HH:MM:SS] PROJ-123: [From] → [To] (by [Author])
[YYYY-MM-DD HH:MM:SS] PROJ-123: [From] → [To] (by [Author])
...
--------------------------------------------------------------------------------
Total status changes: [Count]
```

## Security Check 🔐

- [ ] `appsettings.local.json` is in `.gitignore`
- [ ] Running `git status` does NOT show `appsettings.local.json`
- [ ] API token is not visible in any committed files
- [ ] No credentials in command history that might be shared

## Common Issues & Solutions 🔧

### Issue: "Error: Jira settings are not configured properly"
**Solution:** Create `appsettings.local.json` with all required fields

### Issue: HTTP 401 (Unauthorized)
**Solution:** 
- Verify email is correct
- Regenerate API token if needed
- Ensure token is copied completely (no extra spaces)

### Issue: HTTP 404 (Not Found)
**Solution:**
- Check issue key spelling
- Verify issue exists in https://prugramin.atlassian.net
- Ensure you have permission to view the issue

### Issue: "No status changes found"
**Solution:** This is normal for issues that have never changed status (newly created issues)

### Issue: Build fails
**Solution:**
- Verify .NET 10 SDK is installed
- Run `dotnet restore` first
- Check for compilation errors in output

## Optional: Create Alias 🎯

For easier access, add to your `.zshrc` or `.bashrc`:

```bash
alias jira-status='cd /Users/tidus4400/Projects/JiraStatusScraper/src/JiraStatusScraper && dotnet run'
```

Then reload: `source ~/.zshrc`

Usage:
```bash
jira-status PROJ-123
```

## Project Health Check ✅

Run these periodically:

```bash
# Check for outdated packages
dotnet list package --outdated

# Update packages (if needed)
dotnet add package Microsoft.Extensions.Http
dotnet add package Microsoft.Extensions.Configuration.Json

# Clean build
dotnet clean
dotnet build
```

## Files Checklist 📁

Verify these files exist:

### Source Code
- [ ] `src/JiraStatusScraper/Program.cs`
- [ ] `src/JiraStatusScraper/Configuration/JiraSettings.cs`
- [ ] `src/JiraStatusScraper/Models/JiraUser.cs`
- [ ] `src/JiraStatusScraper/Models/JiraIssue.cs`
- [ ] `src/JiraStatusScraper/Models/Changelog.cs`
- [ ] `src/JiraStatusScraper/Models/StatusChange.cs`
- [ ] `src/JiraStatusScraper/Services/JiraClient.cs`

### Configuration
- [ ] `src/JiraStatusScraper/appsettings.json`
- [ ] `src/JiraStatusScraper/appsettings.local.json.example`
- [ ] `src/JiraStatusScraper/appsettings.local.json` (created by you, git-ignored)
- [ ] `src/JiraStatusScraper/JiraStatusScraper.csproj`

### Documentation
- [ ] `README.md`
- [ ] `QUICKSTART.md`
- [ ] `IMPLEMENTATION.md`

### Solution
- [ ] `JiraStatusScraper.slnx`
- [ ] `.gitignore` (with appsettings.local.json entry)

## Success Criteria ✅

You're ready to use the application when:
- [x] Project builds without errors
- [ ] Can fetch and display issue details
- [ ] Status changes appear in chronological order
- [ ] Pagination works for issues with many changes
- [ ] Credentials are secure (not in git)
- [ ] Error messages are clear and helpful

## Next Steps 🎯

Once basic functionality works:

1. **Test with various issues:**
   - Issues with no status changes
   - Issues with one status change
   - Issues with many status changes (100+)
   - Different projects in your Jira instance

2. **Consider enhancements:**
   - Export results to CSV
   - Filter by date range
   - Bulk processing multiple issues
   - Add logging for debugging

3. **Share with team:**
   - Document your Jira instance specifics
   - Share the repository (without credentials!)
   - Create team documentation for API token generation

---

## Quick Reference Commands 📝

```bash
# Navigate to project
cd /Users/tidus4400/Projects/JiraStatusScraper/src/JiraStatusScraper

# Run interactive mode
dotnet run

# Run with issue key
dotnet run PROJ-123

# Build project
dotnet build

# Clean build
dotnet clean && dotnet build

# Run with different verbosity
dotnet run PROJ-123 --verbosity detailed

# Check .NET version
dotnet --version

# List installed packages
dotnet list package
```

---

**All set?** Just run `dotnet run PROJ-123` and watch your Jira status changes appear! 🎉

