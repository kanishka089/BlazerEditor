# BlazerEditor - Quick Reference

## Installation

```bash
dotnet add package BlazerEditor
```

## Setup

```csharp
// Program.cs
builder.Services.AddBlazerEditor();
```

```razor
// _Imports.razor
@using BlazerEditor
@using BlazerEditor.Components
@using BlazerEditor.Models
```

## Basic Usage

```razor
<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
    private EditorOptions options = new() 
    { 
        MinHeight = 600 
    };
}
```

## Common Operations

### Export HTML
```csharp
var result = await editor.ExportHtmlAsync();
string html = result.Html;
```

### Save Design
```csharp
var design = await editor.SaveDesignAsync();
string json = JsonSerializer.Serialize(design);
```

### Load Design
```csharp
await editor.LoadDesignAsync(jsonString);
// or
await editor.LoadDesignAsync(designObject);
```

## Configuration

```csharp
var options = new EditorOptions
{
    Appearance = new AppearanceConfig 
    { 
        Theme = "modern_light" // or "modern_dark"
    },
    DisplayMode = DisplayMode.Email,
    MinHeight = 600,
    Features = new FeaturesConfig
    {
        TextEditor = true,
        ImageEditor = true,
        UndoRedo = true,
        Preview = true
    }
};
```

## Events

```razor
<EmailEditor @ref="editor"
             OnReady="OnEditorReady"
             OnExport="OnExportComplete"
             OnSave="OnSaveComplete" />

@code {
    private void OnEditorReady()
    {
        // Editor is ready
    }

    private void OnExportComplete(ExportResult result)
    {
        // Export completed
    }

    private void OnSaveComplete(EmailDesign design)
    {
        // Save completed
    }
}
```

## Components Available

- 📝 **Text** - Rich text content
- 📰 **Heading** - Section headings
- 🖼️ **Image** - Images with links
- 🔘 **Button** - Call-to-action buttons
- ➖ **Divider** - Horizontal separators

## Keyboard Shortcuts (Coming Soon)

- `Ctrl+Z` - Undo
- `Ctrl+Y` - Redo
- `Ctrl+S` - Save
- `Delete` - Delete selected

## API Methods

| Method | Description |
|--------|-------------|
| `ExportHtmlAsync()` | Export to HTML |
| `SaveDesignAsync()` | Save as JSON |
| `LoadDesignAsync(json)` | Load from JSON |
| `LoadDesignAsync(design)` | Load from object |

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Options` | `EditorOptions` | `new()` | Configuration |
| `ShowExportButton` | `bool` | `true` | Show export button |
| `ShowSaveButton` | `bool` | `true` | Show save button |

## Themes

- `modern_light` - Light theme (default)
- `modern_dark` - Dark theme
- `classic` - Classic theme (coming soon)

## Common Patterns

### Save to Database
```csharp
var design = await editor.SaveDesignAsync();
var json = JsonSerializer.Serialize(design);
await db.Templates.AddAsync(new Template 
{ 
    DesignJson = json 
});
await db.SaveChangesAsync();
```

### Load from Database
```csharp
var template = await db.Templates.FindAsync(id);
if (template != null)
{
    await editor.LoadDesignAsync(template.DesignJson);
}
```

### Export and Send Email
```csharp
var result = await editor.ExportHtmlAsync();
await emailService.SendAsync(new Email
{
    To = "user@example.com",
    Subject = "Newsletter",
    HtmlBody = result.Html
});
```

## Troubleshooting

### Editor not showing
- Check if services are registered
- Verify imports in `_Imports.razor`
- Check browser console for errors

### CSS not loading
- Ensure CSS isolation is enabled
- Check for conflicting styles
- Verify component is properly referenced

### Export not working
- Check if editor ref is not null
- Ensure editor is ready before exporting
- Check browser console for errors

## Links

- 📚 [Full Documentation](docs/GettingStarted.md)
- 📖 [API Reference](docs/API.md)
- 💡 [Examples](docs/Examples.md)
- 🐛 [Report Issues](https://github.com/yourusername/blazereditor/issues)

## Support

- GitHub Issues: Bug reports
- Discussions: Questions and ideas
- Email: support@blazereditor.dev

---

**Quick Tip**: Start with the demo application to see all features in action!

```bash
cd Demo
dotnet run
```
