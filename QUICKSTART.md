# Quick Start Guide

## Get up and running in 3 minutes

### Step 1: Get your Jira API token
1. Go to https://id.atlassian.com/manage-profile/security/api-tokens
2. Click "Create API token"
3. Name it "JiraStatusScraper" 
4. **Copy the token** (you won't see it again!)

### Step 2: Create your local configuration file

```bash
cd src/JiraStatusScraper
cp appsettings.local.json.example appsettings.local.json
```

Edit `appsettings.local.json`:
```json
{
  "Jira": {
    "BaseUrl": "https://prugramin.atlassian.net",
    "Email": "your.email@company.com",
    "ApiToken": "paste-your-token-here"
  }
}
```

### Step 3: Run it!

```bash
# With an issue key as argument
dotnet run PROJ-123

# Or interactively
dotnet run
# Then type your issue key when prompted
```

## What you'll see

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

## Troubleshooting

### "Error: Jira settings are not configured properly"
→ Make sure `appsettings.local.json` exists and has all three fields filled in

### HTTP 401 (Unauthorized)
→ Check your email and API token are correct

### HTTP 404 (Not Found)
→ Verify the issue key exists and you have permission to view it

### "No status changes found"
→ The issue has never had its status changed (this is normal for newly created issues)

---

**That's it!** You're now tracking Jira status changes like a pro 🚀

