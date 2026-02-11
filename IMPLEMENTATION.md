# Jira Status Scraper - Implementation Summary

## Overview
A .NET 10 console application that fetches issue changelog data from Jira Cloud REST API, specifically tracking status transitions.

## Project Structure

```
JiraStatusScraper/
├── src/
│   └── JiraStatusScraper/
│       ├── Configuration/
│       │   └── JiraSettings.cs           # Configuration model for Jira settings
│       ├── Models/
│       │   ├── Changelog.cs              # ChangelogResponse, ChangelogHistory, ChangelogItem
│       │   ├── JiraIssue.cs              # JiraIssue, JiraIssueFields, JiraStatus
│       │   ├── JiraUser.cs               # JiraUser model
│       │   └── StatusChange.cs           # StatusChange record for display
│       ├── Services/
│       │   └── JiraClient.cs             # Main HTTP client for Jira API
│       ├── Program.cs                    # Application entry point
│       ├── appsettings.json              # Default configuration (checked in)
│       ├── appsettings.local.json.example # Template for local settings
│       └── JiraStatusScraper.csproj      # Project file
├── README.md                             # Documentation
├── .gitignore                            # Git ignore rules (includes appsettings.local.json)
└── JiraStatusScraper.slnx                # Solution file

```

## Key Features

### 1. **Configuration Management**
- **JiraSettings.cs**: POCO for Jira configuration (BaseUrl, Email, ApiToken)
- **appsettings.json**: Default settings with BaseUrl set to https://prugramin.atlassian.net
- **appsettings.local.json**: Git-ignored file for user credentials
- Uses Microsoft.Extensions.Configuration for loading settings

### 2. **Jira API Models**

#### JiraUser
```csharp
public record JiraUser(string AccountId, string? DisplayName, string? EmailAddress);
```

#### JiraIssue
```csharp
public record JiraIssue(string Id, string Key, JiraIssueFields Fields);
public record JiraIssueFields(string Summary, JiraStatus? Status);
public record JiraStatus(string Id, string Name, string? Description);
```

#### Changelog Models
```csharp
public record ChangelogResponse(int StartAt, int MaxResults, int Total, bool IsLast, List<ChangelogHistory> Values);
public record ChangelogHistory(string Id, JiraUser Author, DateTime Created, List<ChangelogItem> Items);
public record ChangelogItem(string Field, string FieldType, string? FieldId, string? From, string? FromString, string? To, string? ToString);
```

#### StatusChange (Output Model)
```csharp
public record StatusChange(string IssueKey, DateTime Timestamp, string AuthorDisplayName, string? FromStatus, string? ToStatus);
```

### 3. **JiraClient Service**

Three main methods:

#### `GetIssueAsync(string issueKey)`
- Fetches basic issue information
- Endpoint: `GET /rest/api/3/issue/{issueKey}?fields=summary,status`
- Returns: JiraIssue with summary and current status

#### `GetStatusChangesAsync(string issueKey)`
- **Main feature**: Fetches all status changes with automatic pagination
- Endpoint: `GET /rest/api/3/issue/{issueKey}/changelog?startAt={startAt}&maxResults={maxResults}`
- Pagination: Loops until `IsLast == true`
- Filters: Only changelog items where `field == "status"`
- Returns: Ordered list of StatusChange records

#### `GetFullChangelogAsync(string issueKey)`
- Fetches complete changelog (all field changes, not just status)
- Uses same pagination strategy
- Returns: Ordered list of all ChangelogHistory entries

### 4. **Authentication**
- Uses Basic Authentication with email + API token
- Credentials loaded from configuration
- Base64-encoded `{email}:{apiToken}` in Authorization header

### 5. **Dependency Injection**
- Uses Microsoft.Extensions.DependencyInjection
- HttpClient configured via `AddHttpClient<JiraClient>()`
- Sets base URL and authentication headers

### 6. **Console Application Flow**

1. Load configuration from appsettings.json and appsettings.local.json
2. Validate Jira settings are present
3. Configure HttpClient with base URL and authentication
4. Get issue key from command-line args or prompt user
5. Fetch issue details
6. Display issue summary and current status
7. Fetch and display all status changes chronologically
8. Handle errors gracefully

## NuGet Packages

- **Microsoft.Extensions.Http** (10.0.3): Typed HttpClient support
- **Microsoft.Extensions.Configuration.Json** (10.0.3): JSON configuration provider

## Setup Instructions

1. **Create local settings file:**
   ```bash
   cd src/JiraStatusScraper
   cp appsettings.local.json.example appsettings.local.json
   ```

2. **Get Jira API token:**
   - Visit https://id.atlassian.com/manage-profile/security/api-tokens
   - Create new API token
   - Copy the token

3. **Update appsettings.local.json:**
   ```json
   {
     "Jira": {
       "BaseUrl": "https://prugramin.atlassian.net",
       "Email": "your-email@example.com",
       "ApiToken": "your-api-token-here"
     }
   }
   ```

4. **Run the application:**
   ```bash
   dotnet run PROJ-123
   ```
   Or interactively:
   ```bash
   dotnet run
   ```

## API Endpoints Used

### Get Issue
```
GET https://prugramin.atlassian.net/rest/api/3/issue/{issueKey}?fields=summary,status
```

### Get Changelog (Paginated)
```
GET https://prugramin.atlassian.net/rest/api/3/issue/{issueKey}/changelog?startAt={startAt}&maxResults={maxResults}
```

## Pagination Strategy

The changelog endpoint returns paginated results:
```json
{
  "startAt": 0,
  "maxResults": 100,
  "total": 250,
  "isLast": false,
  "values": [...]
}
```

The client automatically:
1. Makes initial request with `startAt=0&maxResults=100`
2. Processes returned changelog entries
3. Checks `isLast` field
4. If `false`, increments `startAt` by number of returned items
5. Repeats until `isLast == true`

This ensures all changelog entries are fetched, even for issues with extensive history.

## Error Handling

- Configuration validation before making any API calls
- HTTP status code validation via `EnsureSuccessStatusCode()`
- Graceful handling of:
  - Missing/invalid configuration
  - Authentication failures (401)
  - Issue not found (404)
  - Network errors
  - Unexpected exceptions

## Security Considerations

- `appsettings.local.json` is in .gitignore to prevent credential leaks
- API token has same permissions as the user account
- Tokens should be rotated regularly
- Never commit credentials to version control

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

## Future Enhancements (Not Implemented)

1. **Bulk Processing**: Fetch changelog for multiple issues via JQL query
2. **Export to CSV/JSON**: Save results to file
3. **Filtering**: Date range filters, specific status transitions
4. **Retry Policy**: Using Polly for transient fault handling
5. **Caching**: Cache issue data to reduce API calls
6. **Progress Indicator**: For large changelogs
7. **Server/Data Center Support**: REST API v2 compatibility

## Testing Recommendations

1. Test with issue that has no status changes
2. Test with issue that has many status changes (>100, to verify pagination)
3. Test with invalid issue key
4. Test with invalid credentials
5. Test with issue you don't have permission to view

## Notes

- Jira Cloud REST API v3 documentation: https://developer.atlassian.com/cloud/jira/platform/rest/v3/
- The `ToString` property in ChangelogItem refers to the "to" value of the change (not .ToString() method)
- Date times are in UTC
- The application uses modern C# features: records, top-level statements, nullable reference types

