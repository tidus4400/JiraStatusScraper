using System.Net.Http.Json;
using System.Text.Json;
using JiraStatusScraper.Configuration;
using JiraStatusScraper.Models;

namespace JiraStatusScraper.Services;

public class JiraClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public JiraClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets basic issue information by issue key (e.g., "PROJ-123").
    /// </summary>
    public async Task<JiraIssue?> GetIssueAsync(string issueKey, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"rest/api/3/issue/{issueKey}?fields=summary,status",
            cancellationToken);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JiraIssue>(JsonOptions, cancellationToken);
    }

    /// <summary>
    /// Gets all status changes for an issue by fetching the full changelog with automatic pagination.
    /// </summary>
    public async Task<List<StatusChange>> GetStatusChangesAsync(string issueKey, CancellationToken cancellationToken = default)
    {
        var statusChanges = new List<StatusChange>();
        var startAt = 0;
        const int maxResults = 100;
        bool isLast;

        do
        {
            var response = await _httpClient.GetAsync(
                $"rest/api/3/issue/{issueKey}/changelog?startAt={startAt}&maxResults={maxResults}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var changelog = await response.Content.ReadFromJsonAsync<ChangelogResponse>(JsonOptions, cancellationToken);

            if (changelog is null)
                break;

            foreach (var history in changelog.Values)
            {
                var statusItems = history.Items.Where(item =>
                    item.Field.Equals("status", StringComparison.OrdinalIgnoreCase));

                foreach (var statusItem in statusItems)
                {
                    statusChanges.Add(new StatusChange(
                        IssueKey: issueKey,
                        Timestamp: history.Created,
                        AuthorDisplayName: history.Author.DisplayName ?? history.Author.AccountId,
                        FromStatus: statusItem.FromString,
                        ToStatus: statusItem.ToStringValue
                    ));
                }
            }

            isLast = changelog.IsLast;
            startAt += changelog.Values.Count;

        } while (!isLast);

        return statusChanges.OrderBy(sc => sc.Timestamp).ToList();
    }

    /// <summary>
    /// Gets all changelog entries (not just status changes) for an issue with automatic pagination.
    /// </summary>
    public async Task<List<ChangelogHistory>> GetFullChangelogAsync(string issueKey, CancellationToken cancellationToken = default)
    {
        var allHistory = new List<ChangelogHistory>();
        var startAt = 0;
        const int maxResults = 100;
        bool isLast;

        do
        {
            var response = await _httpClient.GetAsync(
                $"rest/api/3/issue/{issueKey}/changelog?startAt={startAt}&maxResults={maxResults}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var changelog = await response.Content.ReadFromJsonAsync<ChangelogResponse>(JsonOptions, cancellationToken);

            if (changelog is null)
                break;

            allHistory.AddRange(changelog.Values);

            isLast = changelog.IsLast;
            startAt += changelog.Values.Count;

        } while (!isLast);

        return allHistory.OrderBy(h => h.Created).ToList();
    }
}

