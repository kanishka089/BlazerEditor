using System.Text.RegularExpressions;
using BlazerEditor.Models;

namespace BlazerEditor.Services;

/// <summary>
/// Service for handling merge tag operations
/// </summary>
public class MergeTagService
{
    /// <summary>
    /// Replace merge tags in content with sample values for preview
    /// </summary>
    public static string ReplaceWithSamples(string content, List<MergeTag> mergeTags)
    {
        if (string.IsNullOrEmpty(content) || mergeTags == null || !mergeTags.Any())
            return content;

        var result = content;

        foreach (var tag in mergeTags)
        {
            if (!string.IsNullOrEmpty(tag.Value) && !string.IsNullOrEmpty(tag.Sample))
            {
                // Escape special regex characters in the tag value
                var escapedValue = Regex.Escape(tag.Value);
                result = Regex.Replace(result, escapedValue, tag.Sample, RegexOptions.IgnoreCase);
            }
        }

        return result;
    }

    /// <summary>
    /// Find all merge tags in content
    /// </summary>
    public List<string> FindTagsInContent(string content)
    {
        if (string.IsNullOrEmpty(content))
            return new List<string>();

        // Match patterns like {{tag_name}}
        var pattern = @"\{\{([^}]+)\}\}";
        var matches = Regex.Matches(content, pattern);

        return matches.Select(m => m.Value).Distinct().ToList();
    }

    /// <summary>
    /// Validate merge tag syntax
    /// </summary>
    public bool IsValidTagSyntax(string tag)
    {
        if (string.IsNullOrEmpty(tag))
            return false;

        // Check if it matches {{tag_name}} pattern
        var pattern = @"^\{\{[a-zA-Z0-9_]+\}\}$";
        return Regex.IsMatch(tag, pattern);
    }

    /// <summary>
    /// Get merge tags that are used in the design
    /// </summary>
    public List<MergeTag> GetUsedTags(EmailDesign design, List<MergeTag> availableTags)
    {
        var usedTagValues = new HashSet<string>();

        // Search through all content
        foreach (var row in design.Body.Rows)
        {
            foreach (var column in row.Columns)
            {
                foreach (var content in column.Contents)
                {
                    // Check text content
                    if (!string.IsNullOrEmpty(content.Values.Text))
                    {
                        var tags = FindTagsInContent(content.Values.Text);
                        foreach (var tag in tags)
                            usedTagValues.Add(tag);
                    }

                    // Check button text
                    if (content.Type == "button" && !string.IsNullOrEmpty(content.Values.Text))
                    {
                        var tags = FindTagsInContent(content.Values.Text);
                        foreach (var tag in tags)
                            usedTagValues.Add(tag);
                    }

                    // Check links
                    if (content.Values.Href?.Values?.Href != null)
                    {
                        var tags = FindTagsInContent(content.Values.Href.Values.Href);
                        foreach (var tag in tags)
                            usedTagValues.Add(tag);
                    }
                }
            }
        }

        // Return only the tags that are actually used
        return availableTags.Where(t => usedTagValues.Contains(t.Value)).ToList();
    }

    /// <summary>
    /// Highlight merge tags in HTML for visual display
    /// </summary>
    public string HighlightTags(string content)
    {
        if (string.IsNullOrEmpty(content))
            return content;

        // Wrap merge tags in span with class for styling
        var pattern = @"(\{\{[^}]+\}\})";
        return Regex.Replace(content, pattern, "<span class=\"merge-tag-highlight\">$1</span>");
    }

    /// <summary>
    /// Convert ESP-specific syntax to standard format
    /// </summary>
    public string ConvertToStandardSyntax(string content, string espFormat)
    {
        if (string.IsNullOrEmpty(content))
            return content;

        return espFormat.ToLower() switch
        {
            "mailchimp" => ConvertMailchimpToStandard(content),
            "sendgrid" => content, // Already uses {{tag}} format
            "campaignmonitor" => ConvertCampaignMonitorToStandard(content),
            _ => content
        };
    }

    /// <summary>
    /// Convert standard syntax to ESP-specific format
    /// </summary>
    public string ConvertFromStandardSyntax(string content, string espFormat, List<MergeTag> mergeTags)
    {
        if (string.IsNullOrEmpty(content))
            return content;

        return espFormat.ToLower() switch
        {
            "mailchimp" => ConvertStandardToMailchimp(content, mergeTags),
            "sendgrid" => content, // Already uses {{tag}} format
            "campaignmonitor" => ConvertStandardToCampaignMonitor(content, mergeTags),
            _ => content
        };
    }

    private string ConvertMailchimpToStandard(string content)
    {
        // Convert *|TAG|* to {{tag}}
        return Regex.Replace(content, @"\*\|([^|]+)\|\*", "{{$1}}");
    }

    private string ConvertStandardToMailchimp(string content, List<MergeTag> mergeTags)
    {
        // Convert {{tag}} to *|TAG|*
        foreach (var tag in mergeTags)
        {
            var standardTag = tag.Value;
            var mailchimpTag = $"*|{tag.Key.ToUpper()}|*";
            content = content.Replace(standardTag, mailchimpTag);
        }
        return content;
    }

    private string ConvertCampaignMonitorToStandard(string content)
    {
        // Convert [tag] to {{tag}}
        return Regex.Replace(content, @"\[([^\]]+)\]", "{{$1}}");
    }

    private string ConvertStandardToCampaignMonitor(string content, List<MergeTag> mergeTags)
    {
        // Convert {{tag}} to [tag]
        foreach (var tag in mergeTags)
        {
            var standardTag = tag.Value;
            var cmTag = $"[{tag.Key}]";
            content = content.Replace(standardTag, cmTag);
        }
        return content;
    }
}
