using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JiraStatusScraper.Configuration;
using JiraStatusScraper.Services;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
    .Build();

var jiraSettings = configuration.GetSection("Jira").Get<JiraSettings>();

if (jiraSettings is null || string.IsNullOrWhiteSpace(jiraSettings.BaseUrl) ||
    string.IsNullOrWhiteSpace(jiraSettings.Email) || string.IsNullOrWhiteSpace(jiraSettings.ApiToken))
{
    Console.WriteLine("Error: Jira settings are not configured properly.");
    Console.WriteLine("Please update appsettings.json or create appsettings.local.json with your Jira credentials.");
    return 1;
}

// Setup DI and HttpClient
var services = new ServiceCollection();

services.AddHttpClient<JiraClient>(client =>
{
    client.BaseAddress = new Uri(jiraSettings.BaseUrl.TrimEnd('/') + "/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
    // Basic auth with email:apiToken
    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{jiraSettings.Email}:{jiraSettings.ApiToken}"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
});

var serviceProvider = services.BuildServiceProvider();
var jiraClient = serviceProvider.GetRequiredService<JiraClient>();

// Get issue key from command line args or prompt
string? issueKey = args.Length > 0 ? args[0] : null;

if (string.IsNullOrWhiteSpace(issueKey))
{
    Console.Write("Enter Jira issue key (e.g., PROJ-123): ");
    issueKey = Console.ReadLine();
}

if (string.IsNullOrWhiteSpace(issueKey))
{
    Console.WriteLine("Error: No issue key provided.");
    return 1;
}

try
{
    Console.WriteLine($"\nFetching issue {issueKey}...\n");

    // Get issue details
    var issue = await jiraClient.GetIssueAsync(issueKey);
    
    if (issue is null)
    {
        Console.WriteLine($"Error: Issue {issueKey} not found.");
        return 1;
    }

    Console.WriteLine($"Issue: {issue.Key} - {issue.Fields.Summary}");
    Console.WriteLine($"Current Status: {issue.Fields.Status?.Name ?? "Unknown"}");
    Console.WriteLine();

    // Get status changes
    Console.WriteLine("Status Change History:");
    Console.WriteLine(new string('-', 80));

    var statusChanges = await jiraClient.GetStatusChangesAsync(issueKey);

    if (statusChanges.Count == 0)
    {
        Console.WriteLine("No status changes found.");
    }
    else
    {
        foreach (var change in statusChanges)
        {
            Console.WriteLine(change);
        }
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Total status changes: {statusChanges.Count}");
    }
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Error connecting to Jira: {ex.Message}");
    return 1;
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
    return 1;
}

return 0;
