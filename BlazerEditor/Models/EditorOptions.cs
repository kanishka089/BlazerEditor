namespace BlazerEditor.Models;

/// <summary>
/// Configuration options for the email editor
/// </summary>
public class EditorOptions
{
    /// <summary>
    /// Appearance configuration (theme, colors, etc.)
    /// </summary>
    public AppearanceConfig? Appearance { get; set; }

    /// <summary>
    /// Display mode: Email or Web
    /// </summary>
    public DisplayMode DisplayMode { get; set; } = DisplayMode.Email;

    /// <summary>
    /// Locale for internationalization (e.g., "en-US", "es-ES")
    /// </summary>
    public string Locale { get; set; } = "en-US";

    /// <summary>
    /// Custom tools configuration
    /// </summary>
    public ToolsConfig? Tools { get; set; }

    /// <summary>
    /// Minimum height of the editor in pixels
    /// </summary>
    public int MinHeight { get; set; } = 600;

    /// <summary>
    /// Enable/disable features
    /// </summary>
    public FeaturesConfig? Features { get; set; }

    /// <summary>
    /// Custom merge tags for personalization
    /// </summary>
    public List<MergeTag>? MergeTags { get; set; }

    /// <summary>
    /// Enable merge tags feature
    /// </summary>
    public bool EnableMergeTags { get; set; } = true;

    /// <summary>
    /// Show merge tag preview mode toggle
    /// </summary>
    public bool ShowMergeTagPreview { get; set; } = true;
}

public class AppearanceConfig
{
    /// <summary>
    /// Theme: modern_light, modern_dark, classic
    /// </summary>
    public string Theme { get; set; } = "modern_light";

    /// <summary>
    /// Custom panel colors
    /// </summary>
    public Dictionary<string, string>? Panels { get; set; }
}

public class ToolsConfig
{
    /// <summary>
    /// Enable/disable specific tools
    /// </summary>
    public Dictionary<string, bool>? Enabled { get; set; }

    /// <summary>
    /// Custom tool properties
    /// </summary>
    public Dictionary<string, object>? Properties { get; set; }
}

public class FeaturesConfig
{
    public bool TextEditor { get; set; } = true;
    public bool ImageEditor { get; set; } = true;
    public bool UndoRedo { get; set; } = true;
    public bool Preview { get; set; } = true;
    public bool Export { get; set; } = true;
    public bool Import { get; set; } = true;
}

// MergeTag class moved to separate file: Models/MergeTag.cs

public enum DisplayMode
{
    Email,
    Web
}
