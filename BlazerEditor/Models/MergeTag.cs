namespace BlazerEditor.Models;

/// <summary>
/// Represents a merge tag (personalization variable) that can be inserted into email content
/// </summary>
public class MergeTag
{
    /// <summary>
    /// Unique identifier for the merge tag
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Display name shown in the UI
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The actual placeholder value (e.g., "{{first_name}}")
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Sample value shown in preview mode
    /// </summary>
    public string Sample { get; set; } = string.Empty;

    /// <summary>
    /// Category for grouping tags (e.g., "Personal", "Company", "Links")
    /// </summary>
    public string Category { get; set; } = "General";

    /// <summary>
    /// Optional description or help text
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Represents a group of merge tags (Unlayer-style grouping)
/// </summary>
public class MergeTagGroup
{
    /// <summary>
    /// Group name (e.g., "Contract", "Person", "Realestate")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Array of merge tags in this group
    /// </summary>
    public List<MergeTag> MergeTags { get; set; } = new();
}

/// <summary>
/// Predefined merge tag categories
/// </summary>
public static class MergeTagCategories
{
    public const string Personal = "Personal";
    public const string Company = "Company";
    public const string Links = "Links";
    public const string System = "System";
    public const string Custom = "Custom";
}

/// <summary>
/// Helper class to create common merge tags
/// </summary>
public static class MergeTagDefaults
{
    public static List<MergeTag> GetDefaultTags()
    {
        return new List<MergeTag>
        {
            // Personal
            new MergeTag
            {
                Key = "first_name",
                Name = "First Name",
                Value = "{{first_name}}",
                Sample = "John",
                Category = MergeTagCategories.Personal,
                Description = "Recipient's first name"
            },
            new MergeTag
            {
                Key = "last_name",
                Name = "Last Name",
                Value = "{{last_name}}",
                Sample = "Doe",
                Category = MergeTagCategories.Personal,
                Description = "Recipient's last name"
            },
            new MergeTag
            {
                Key = "full_name",
                Name = "Full Name",
                Value = "{{full_name}}",
                Sample = "John Doe",
                Category = MergeTagCategories.Personal,
                Description = "Recipient's full name"
            },
            new MergeTag
            {
                Key = "email",
                Name = "Email Address",
                Value = "{{email}}",
                Sample = "john.doe@example.com",
                Category = MergeTagCategories.Personal,
                Description = "Recipient's email address"
            },
            
            // Company
            new MergeTag
            {
                Key = "company",
                Name = "Company Name",
                Value = "{{company}}",
                Sample = "Acme Corporation",
                Category = MergeTagCategories.Company,
                Description = "Recipient's company name"
            },
            new MergeTag
            {
                Key = "job_title",
                Name = "Job Title",
                Value = "{{job_title}}",
                Sample = "Marketing Manager",
                Category = MergeTagCategories.Company,
                Description = "Recipient's job title"
            },
            
            // Links
            new MergeTag
            {
                Key = "unsubscribe_url",
                Name = "Unsubscribe Link",
                Value = "{{unsubscribe_url}}",
                Sample = "https://example.com/unsubscribe",
                Category = MergeTagCategories.Links,
                Description = "Link to unsubscribe from emails"
            },
            new MergeTag
            {
                Key = "view_online_url",
                Name = "View Online Link",
                Value = "{{view_online_url}}",
                Sample = "https://example.com/view",
                Category = MergeTagCategories.Links,
                Description = "Link to view email in browser"
            },
            new MergeTag
            {
                Key = "preferences_url",
                Name = "Preferences Link",
                Value = "{{preferences_url}}",
                Sample = "https://example.com/preferences",
                Category = MergeTagCategories.Links,
                Description = "Link to email preferences"
            },
            
            // System
            new MergeTag
            {
                Key = "current_date",
                Name = "Current Date",
                Value = "{{current_date}}",
                Sample = DateTime.Now.ToString("MMMM dd, yyyy"),
                Category = MergeTagCategories.System,
                Description = "Current date"
            },
            new MergeTag
            {
                Key = "current_year",
                Name = "Current Year",
                Value = "{{current_year}}",
                Sample = DateTime.Now.Year.ToString(),
                Category = MergeTagCategories.System,
                Description = "Current year"
            }
        };
    }
}
