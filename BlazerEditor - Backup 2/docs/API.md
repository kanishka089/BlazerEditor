# API Reference

## EmailEditor Component

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Options` | `EditorOptions` | `new()` | Configuration options for the editor |
| `OnReady` | `EventCallback` | - | Fired when editor is initialized |
| `OnExport` | `EventCallback<ExportResult>` | - | Fired when export is triggered |
| `OnSave` | `EventCallback<EmailDesign>` | - | Fired when save is triggered |
| `ShowExportButton` | `bool` | `true` | Show/hide export button in toolbar |
| `ShowSaveButton` | `bool` | `true` | Show/hide save button in toolbar |

### Methods

#### ExportHtmlAsync()

Exports the current design as HTML.

```csharp
Task<ExportResult> ExportHtmlAsync()
```

**Returns:** `ExportResult` containing HTML string and design JSON.

**Example:**
```csharp
var result = await editorRef.ExportHtmlAsync();
Console.WriteLine(result.Html);
```

#### SaveDesignAsync()

Saves the current design as JSON.

```csharp
Task<EmailDesign> SaveDesignAsync()
```

**Returns:** `EmailDesign` object.

**Example:**
```csharp
var design = await editorRef.SaveDesignAsync();
var json = JsonSerializer.Serialize(design);
```

#### LoadDesignAsync(string json)

Loads a design from JSON string.

```csharp
Task LoadDesignAsync(string designJson)
```

**Parameters:**
- `designJson`: JSON string representing the email design

**Example:**
```csharp
await editorRef.LoadDesignAsync(savedJson);
```

#### LoadDesignAsync(EmailDesign design)

Loads a design from EmailDesign object.

```csharp
Task LoadDesignAsync(EmailDesign design)
```

**Parameters:**
- `design`: EmailDesign object

**Example:**
```csharp
var design = new EmailDesign { /* ... */ };
await editorRef.LoadDesignAsync(design);
```

## Models

### EditorOptions

Configuration options for the editor.

```csharp
public class EditorOptions
{
    public AppearanceConfig? Appearance { get; set; }
    public DisplayMode DisplayMode { get; set; } = DisplayMode.Email;
    public string Locale { get; set; } = "en-US";
    public ToolsConfig? Tools { get; set; }
    public int MinHeight { get; set; } = 600;
    public FeaturesConfig? Features { get; set; }
    public List<MergeTag>? MergeTags { get; set; }
}
```

### EmailDesign

Represents the complete email design structure.

```csharp
public class EmailDesign
{
    public Dictionary<string, int> Counters { get; set; }
    public EmailBody Body { get; set; }
    public int SchemaVersion { get; set; } = 1;
}
```

### ExportResult

Result of HTML export operation.

```csharp
public class ExportResult
{
    public string Html { get; set; }
    public EmailDesign Design { get; set; }
    public DateTime ExportedAt { get; set; }
}
```

### AppearanceConfig

Appearance configuration.

```csharp
public class AppearanceConfig
{
    public string Theme { get; set; } = "modern_light";
    public Dictionary<string, string>? Panels { get; set; }
}
```

**Available Themes:**
- `modern_light` - Modern light theme (default)
- `modern_dark` - Modern dark theme
- `classic` - Classic theme

### DisplayMode

```csharp
public enum DisplayMode
{
    Email,
    Web
}
```

### FeaturesConfig

Enable/disable specific features.

```csharp
public class FeaturesConfig
{
    public bool TextEditor { get; set; } = true;
    public bool ImageEditor { get; set; } = true;
    public bool UndoRedo { get; set; } = true;
    public bool Preview { get; set; } = true;
    public bool Export { get; set; } = true;
    public bool Import { get; set; } = true;
}
```

### MergeTag

Custom merge tag for personalization.

```csharp
public class MergeTag
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string? Sample { get; set; }
}
```

## Services

### HtmlExportService

Service for exporting email designs to HTML.

```csharp
public class HtmlExportService
{
    public string ExportToHtml(EmailDesign design)
}
```

### DesignService

Service for managing email designs.

```csharp
public class DesignService
{
    public string SerializeDesign(EmailDesign design)
    public EmailDesign? DeserializeDesign(string json)
    public EmailDesign CreateEmptyDesign()
    public void IncrementCounter(EmailDesign design, string counterName)
}
```

## Events

### OnReady

Fired when the editor is fully initialized and ready to use.

```csharp
private void OnEditorReady()
{
    Console.WriteLine("Editor is ready!");
}
```

### OnExport

Fired when the export operation completes.

```csharp
private void OnExportComplete(ExportResult result)
{
    Console.WriteLine($"Exported: {result.Html.Length} characters");
}
```

### OnSave

Fired when the save operation completes.

```csharp
private void OnSaveComplete(EmailDesign design)
{
    Console.WriteLine($"Saved design with {design.Body.Rows.Count} rows");
}
```
