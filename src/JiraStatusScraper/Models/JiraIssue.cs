namespace JiraStatusScraper.Models;

public record JiraIssue(
    string Id,
    string Key,
    JiraIssueFields Fields
);

public record JiraIssueFields(
    string Summary,
    JiraStatus? Status
);

public record JiraStatus(
    string Id,
    string Name,
    string? Description
);

