# Jira Status Scraper

A modern .NET 10 console application to fetch and display status change history from Jira Cloud REST API.

## Features

- Fetches issue changelog from Jira Cloud (https://prugramin.atlassian.net)
- Displays all status transitions for a given issue
- Automatic pagination for issues with large changelogs
- Simple console interface

## Prerequisites

- .NET 10 SDK
- Jira Cloud account with API access
- Jira API token (see setup below)

## Setup

### 1. Get Your Jira API Token

1. Log in to https://id.atlassian.com/manage-profile/security/api-tokens
2. Click "Create API token"
3. Give it a name (e.g., "Status Scraper")
4. Copy the generated token

### 2. Configure Credentials

Copy `appsettings.local.json.example` to `appsettings.local.json`:

```bash
cd src/JiraStatusScraper
cp appsettings.local.json.example appsettings.local.json
```

Edit `appsettings.local.json` and add your credentials:

```json
{
  "Jira": {
    "BaseUrl": "https://prugramin.atlassian.net",
    "Email": "your-email@example.com",
    "ApiToken": "your-api-token-here"
  }
}
```

**Note:** `appsettings.local.json` is git-ignored to protect your credentials.

## Usage

### Run with Issue Key as Argument

```bash
dotnet run PROJ-123
```

### Run Interactively

```bash
dotnet run
```

Then enter the issue key when prompted.

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

## Project Structure

```
JiraStatusScraper/
├── Configuration/
│   └── JiraSettings.cs          # Configuration model
├── Models/
│   ├── JiraUser.cs              # User model
│   ├── JiraIssue.cs             # Issue and status models
│   ├── Changelog.cs             # Changelog API response models
│   └── StatusChange.cs          # Status change representation
├── Services/
│   └── JiraClient.cs            # Main Jira API client
├── Program.cs                   # Application entry point
└── appsettings.json             # Default configuration
```

## API Methods

### JiraClient

- `GetIssueAsync(string issueKey)` - Fetch basic issue information
- `GetStatusChangesAsync(string issueKey)` - Fetch all status transitions with automatic pagination
- `GetFullChangelogAsync(string issueKey)` - Fetch complete changelog (all field changes)

## Security Notes

- Never commit `appsettings.local.json` to version control
- Keep your API token secure
- The API token has the same permissions as your user account

## Troubleshooting

### Authentication Error

If you get a 401 error, verify:
- Email is correct in `appsettings.local.json`
- API token is valid and not expired
- Token has proper permissions

### Issue Not Found (404)

- Verify the issue key exists in your Jira instance
- Ensure you have permission to view the issue

## License

MIT

