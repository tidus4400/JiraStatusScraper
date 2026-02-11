using System.Text.Json.Serialization;

namespace JiraStatusScraper.Models;

/// <summary>
/// Represents the paginated changelog response from Jira API.
/// </summary>
public record ChangelogResponse(
    int StartAt,
    int MaxResults,
    int Total,
    bool IsLast,
    List<ChangelogHistory> Values
);

/// <summary>
/// Represents a single changelog entry (a set of changes made at one time).
/// </summary>
public record ChangelogHistory(
    string Id,
    JiraUser Author,
    DateTime Created,
    List<ChangelogItem> Items
);

/// <summary>
/// Represents a single field change within a changelog entry.
/// </summary>
public record ChangelogItem(
    string Field,
    string FieldType,
    string? FieldId,
    string? From,
    string? FromString,
    string? To,
    [property: JsonPropertyName("toString")] string? ToStringValue
);

