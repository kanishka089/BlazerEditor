using System.Text.Json.Serialization;

namespace BlazerEditor.Models;

/// <summary>
/// Represents the complete email design structure
/// </summary>
public class EmailDesign
{
    [JsonPropertyName("counters")]
    public Dictionary<string, int> Counters { get; set; } = new();

    [JsonPropertyName("body")]
    public EmailBody Body { get; set; } = new();

    [JsonPropertyName("schemaVersion")]
    public int SchemaVersion { get; set; } = 1;
}

public class EmailBody
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..10];

    [JsonPropertyName("rows")]
    public List<Row> Rows { get; set; } = new();

    [JsonPropertyName("values")]
    public BodyValues Values { get; set; } = new();
}

public class BodyValues
{
    [JsonPropertyName("backgroundColor")]
    public string BackgroundColor { get; set; } = "#f9f9f9";

    [JsonPropertyName("contentWidth")]
    public string ContentWidth { get; set; } = "600px";

    [JsonPropertyName("fontFamily")]
    public FontFamily FontFamily { get; set; } = new();

    [JsonPropertyName("preheaderText")]
    public string PreheaderText { get; set; } = string.Empty;
}

public class FontFamily
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = "Arial";

    [JsonPropertyName("value")]
    public string Value { get; set; } = "arial,helvetica,sans-serif";
}

public class Row
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..10];

    [JsonPropertyName("cells")]
    public List<int> Cells { get; set; } = new();

    [JsonPropertyName("columns")]
    public List<Column> Columns { get; set; } = new();

    [JsonPropertyName("values")]
    public RowValues Values { get; set; } = new();
}

public class RowValues
{
    [JsonPropertyName("backgroundColor")]
    public string BackgroundColor { get; set; } = string.Empty;

    [JsonPropertyName("padding")]
    public string Padding { get; set; } = "10px";

    [JsonPropertyName("columnsBackgroundColor")]
    public string ColumnsBackgroundColor { get; set; } = string.Empty;

    [JsonPropertyName("backgroundImage")]
    public BackgroundImage? BackgroundImage { get; set; }

    [JsonPropertyName("hideDesktop")]
    public bool HideDesktop { get; set; }

    [JsonPropertyName("hideMobile")]
    public bool HideMobile { get; set; }

    [JsonPropertyName("displayCondition")]
    public string? DisplayCondition { get; set; }
}

public class BackgroundImage
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("fullWidth")]
    public bool FullWidth { get; set; } = true;

    [JsonPropertyName("repeat")]
    public string Repeat { get; set; } = "no-repeat";

    [JsonPropertyName("size")]
    public string Size { get; set; } = "cover";

    [JsonPropertyName("position")]
    public string Position { get; set; } = "center";
}

public class Column
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..10];

    [JsonPropertyName("contents")]
    public List<Content> Contents { get; set; } = new();

    [JsonPropertyName("values")]
    public ColumnValues Values { get; set; } = new();
}

public class ColumnValues
{
    [JsonPropertyName("backgroundColor")]
    public string BackgroundColor { get; set; } = string.Empty;

    [JsonPropertyName("padding")]
    public string Padding { get; set; } = "0px";

    [JsonPropertyName("border")]
    public Border Border { get; set; } = new();
}

public class Border
{
    [JsonPropertyName("borderTopWidth")]
    public string BorderTopWidth { get; set; } = "0px";

    [JsonPropertyName("borderTopStyle")]
    public string BorderTopStyle { get; set; } = "solid";

    [JsonPropertyName("borderTopColor")]
    public string BorderTopColor { get; set; } = "#000000";
}

public class Content
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..10];

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("values")]
    public ContentValues Values { get; set; } = new();

    [JsonPropertyName("columns")]
    public List<Column>? Columns { get; set; }
}

public class ContentValues
{
    [JsonPropertyName("containerPadding")]
    public string ContainerPadding { get; set; } = "10px";

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("fontSize")]
    public string FontSize { get; set; } = "14px";

    [JsonPropertyName("textAlign")]
    public string TextAlign { get; set; } = "left";

    [JsonPropertyName("lineHeight")]
    public string LineHeight { get; set; } = "140%";

    [JsonPropertyName("color")]
    public string Color { get; set; } = "#000000";

    [JsonPropertyName("src")]
    public ImageSource? Src { get; set; }

    [JsonPropertyName("altText")]
    public string AltText { get; set; } = string.Empty;

    [JsonPropertyName("href")]
    public LinkAction? Href { get; set; }

    [JsonPropertyName("buttonColors")]
    public ButtonColors? ButtonColors { get; set; }

    [JsonPropertyName("hideDesktop")]
    public bool HideDesktop { get; set; }

    [JsonPropertyName("hideMobile")]
    public bool HideMobile { get; set; }
}

public class ImageSource
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("maxWidth")]
    public string MaxWidth { get; set; } = "100%";

    [JsonPropertyName("autoWidth")]
    public bool AutoWidth { get; set; } = true;
}

public class LinkAction
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "web";

    [JsonPropertyName("values")]
    public LinkValues Values { get; set; } = new();
}

public class LinkValues
{
    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    [JsonPropertyName("target")]
    public string Target { get; set; } = "_blank";
}

public class ButtonColors
{
    [JsonPropertyName("color")]
    public string Color { get; set; } = "#FFFFFF";

    [JsonPropertyName("backgroundColor")]
    public string BackgroundColor { get; set; } = "#3AAEE0";

    [JsonPropertyName("hoverColor")]
    public string HoverColor { get; set; } = "#FFFFFF";

    [JsonPropertyName("hoverBackgroundColor")]
    public string HoverBackgroundColor { get; set; } = "#2A8EC0";
}
