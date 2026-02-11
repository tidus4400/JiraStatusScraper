namespace JiraStatusScraper.Models;

public record JiraUser(
    string AccountId,
    string? DisplayName,
    string? EmailAddress
);

