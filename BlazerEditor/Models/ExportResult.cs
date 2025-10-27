namespace BlazerEditor.Models;

/// <summary>
/// Result of HTML export operation
/// </summary>
public class ExportResult
{
    /// <summary>
    /// Generated HTML content ready to send as email
    /// </summary>
    public string Html { get; set; } = string.Empty;

    /// <summary>
    /// Design object (use DesignJson for storage)
    /// </summary>
    public EmailDesign Design { get; set; } = new();

    /// <summary>
    /// Design as JSON string - save this to your database
    /// </summary>
    public string DesignJson { get; set; } = string.Empty;

    /// <summary>
    /// Whether the export was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if export failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Export timestamp
    /// </summary>
    public DateTime ExportedAt { get; set; } = DateTime.UtcNow;
}
