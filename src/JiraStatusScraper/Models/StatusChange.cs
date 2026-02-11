namespace JiraStatusScraper.Models;

/// <summary>
/// Represents a status change event extracted from the changelog.
/// </summary>
public record StatusChange(
    string IssueKey,
    DateTime Timestamp,
    string AuthorDisplayName,
    string? FromStatus,
    string? ToStatus
)
{
    public override string ToString() =>
        $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {IssueKey}: {FromStatus ?? "(none)"} → {ToStatus ?? "(none)"} (by {AuthorDisplayName})";
}

