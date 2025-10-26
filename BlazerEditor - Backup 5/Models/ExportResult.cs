namespace BlazerEditor.Models;

/// <summary>
/// Result of HTML export operation
/// </summary>
public class ExportResult
{
    /// <summary>
    /// Generated HTML content
    /// </summary>
    public string Html { get; set; } = string.Empty;

    /// <summary>
    /// Design JSON
    /// </summary>
    public EmailDesign Design { get; set; } = new();

    /// <summary>
    /// Export timestamp
    /// </summary>
    public DateTime ExportedAt { get; set; } = DateTime.UtcNow;
}
